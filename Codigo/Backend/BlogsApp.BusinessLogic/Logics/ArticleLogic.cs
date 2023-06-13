using BlogsApp.Domain.Entities;
using BlogsApp.Domain.Exceptions;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using System.Data;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using System.Globalization;

namespace BlogsApp.BusinessLogic.Logics
{
    public class ArticleLogic : IArticleLogic
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentLogic _commentLogic;
        private readonly IOffensiveWordsValidator _offensiveWordsValidator;

        public ArticleLogic(IArticleRepository articleRepository, ICommentLogic commentLogic, IOffensiveWordsValidator offensiveWordsValidator)
        {
            _articleRepository = articleRepository;
            _commentLogic = commentLogic;
            _offensiveWordsValidator = offensiveWordsValidator;
        }

        public Article CreateArticle(Article article, User loggedUser)
        {
            if (loggedUser.Blogger && isValidArticle(article))
            {
                List<string> offensiveWordsFound = _offensiveWordsValidator.reviewArticle(article);
                if (offensiveWordsFound.Count() > 0)
                {
                    article.State = Domain.Enums.ContentState.InReview;
                    article.OffensiveWords = OffensiveWordsValidatorUtils.mapToOffensiveWordsType(offensiveWordsFound);
                    _offensiveWordsValidator.NotifyAdminsAndModerators();
                }

                this._articleRepository.Add(article);
                
                return article;
            } else
            {
                throw new UnauthorizedAccessException("Sólo los Bloggers pueden crear artículos");
            }
        }

        public void DeleteArticle(int articleId, User loggedUser)
        {
            Article article = _articleRepository.Get(ArticleById(articleId, loggedUser));
            if(loggedUser.Id == article.UserId)
            {
                foreach (Comment comment in article.Comments)
                {
                    _commentLogic.DeleteComment(comment.Id, loggedUser);
                }
                article.DateDeleted = DateTime.Now;
                article.State = Domain.Enums.ContentState.Deleted;
                this._articleRepository.Update(article);
            } else
            {
                throw new UnauthorizedAccessException("Sólo el creador del artículo puede eliminarlo");
            };
        }

        public Article GetArticleById(int id, User loggedUser)
        {
            return _articleRepository.Get(ArticleById(id, loggedUser));
        }

        public IEnumerable<Article> GetArticles(User loggedUser, string? searchText)
        {
            if (searchText == null)
            {
                return _articleRepository.GetAll(m => m.DateDeleted == null
                                    && (m.State == Domain.Enums.ContentState.Visible || m.State == Domain.Enums.ContentState.Edited)
                                    && (m.Private == false || m.UserId == loggedUser.Id))
                                 .OrderByDescending(m => m.DateModified)
                                 .Take(10);
            }
            else
            {
                return _articleRepository.GetAll(ArticleByTextSearch(searchText, loggedUser));
            }
        }

        public IEnumerable<Article> GetArticlesByUser(int userId, User loggedUser)
        {
            return _articleRepository.GetAll(m => m.DateDeleted == null && m.UserId == userId
                                            && (m.State == Domain.Enums.ContentState.Visible || m.State == Domain.Enums.ContentState.Edited)
                                            && (!m.Private || m.UserId == loggedUser.Id));
        }

        public IDictionary<string, int> GetStatsByYear(int year, User loggedUser)
        {
            if(loggedUser.Admin)
            {
                var months = Enumerable.Range(1, 12);

                return months.GroupJoin(
                                    _articleRepository.GetAll(a => a.DateCreated.Year == year),
                                    month => month,
                                    article => article.DateCreated.Month,
                                    (month, articlesGroup) => new { Month = month, ArticleCount = articlesGroup.Count() }
                                )
                                .ToDictionary(
                                    item => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Month),
                                    item => item.ArticleCount
                                );
            } else
            {
                throw new UnauthorizedAccessException("Se necesitan permisos de administrador");
            }
        }

        public Article UpdateArticle(int articleId, Article anArticle, User loggedUser)
        {
            Article article = _articleRepository.Get(ArticleById(articleId, loggedUser));
            isValidArticle(anArticle);

            List<string> offensiveWordsFound = _offensiveWordsValidator.reviewArticle(anArticle);
            if (offensiveWordsFound.Count() > 0)
            {
                article.State = Domain.Enums.ContentState.InReview;
                _offensiveWordsValidator.NotifyAdminsAndModerators();
            }
            else if (article.State == Domain.Enums.ContentState.InReview || article.State == Domain.Enums.ContentState.Visible)
            {
                // si un contenido entra acá es porque o estaba en revisión y fue editado (se le quitaron las palabras ofensivas en la edición), o estaba publicado normal y fue editado y la edición no cuenta con palabras ofensivas
                article.State = Domain.Enums.ContentState.Edited;
            }

            if (loggedUser.Id == article.UserId || loggedUser.Admin || loggedUser.Moderador)
            {
                article.Name = anArticle.Name;
                article.Body = anArticle.Body;
                article.Private = anArticle.Private;
                article.DateModified = DateTime.Now;
                article.Template = anArticle.Template;
                article.Image = anArticle.Image;
                // si no se encontraron palabras ofensivas la lista se guarda vacia, de lo contrario se actualizan las encontradas:
                article.OffensiveWords = OffensiveWordsValidatorUtils.mapToOffensiveWordsType(offensiveWordsFound);
                this._articleRepository.Update(article);
                return article;
            } else
            {
                throw new UnauthorizedAccessException("Sólo el creador del artículo o un moderador pueden modificarlo");
            };
        }

        private Func<Article, bool> ArticleById(int id, User loggedUser)
        {
            return a => a.Id == id && a.DateDeleted == null && (a.State == Domain.Enums.ContentState.Visible || a.State == Domain.Enums.ContentState.Edited) && (!a.Private || a.UserId == loggedUser.Id);
        }

        private Func<Article, bool> ArticleByTextSearch(string text, User loggedUser)
        {
            return article => article.DateDeleted == null &&
                              (article.State == Domain.Enums.ContentState.Visible || article.State == Domain.Enums.ContentState.Edited) &&
                              (article.Name.Contains(text) || article.Body.Contains(text)) &&
                              (article.Private == false || article.UserId == loggedUser.Id);
        }

        public bool isValidArticle(Article? article)
        {
            if (article == null)
            {
                throw new BadInputException("Articulo inválido");
            }
            if (article.Name == null || article.Name == "")
            {
                throw new BadInputException("Debe ingresar un nombre");
            }
            if (article.Body == null || article.Body == "")
            {
                throw new BadInputException("Debe ingresar una contenido");
            }
            if (article.User == null)
            {
                throw new BadInputException("El articulo debe contener un autor");
            }
            if (article.Template == null)
            {
                throw new BadInputException("Debe ingresar un template válido");
            }
            return true;
        }



    }
}

