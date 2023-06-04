﻿using BlogsApp.Domain.Entities;
using BlogsApp.Domain.Exceptions;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using System.Data;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;
using BlogsApp.DataAccess.Interfaces.Exceptions;

namespace BlogsApp.BusinessLogic.Logics
{
    public class ArticleLogic : IArticleLogic
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserLogic _userLogic;
        private readonly ICommentLogic _commentLogic;

        public ArticleLogic(IArticleRepository articleRepository, ICommentLogic commentLogic, IUserLogic userLogic)
        {
            _articleRepository = articleRepository;
            _commentLogic = commentLogic;
            _userLogic = userLogic;
        }

        public Article CreateArticle(Article article, User loggedUser)
        {
            if (loggedUser.Blogger && isValidArticle(article))
            {
                this._articleRepository.Add(article);
                loggedUser.Articles.Add(article);
                this._userLogic.UpdateUser(loggedUser, loggedUser);
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
                return _articleRepository.GetAll(m => m.DateDeleted == null && (m.Private == false || m.UserId == loggedUser.Id))
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
            return _articleRepository.GetAll(m => m.DateDeleted == null && m.UserId == userId && (!m.Private || m.UserId == loggedUser.Id));
        }

        public IEnumerable<int> GetStatsByYear(int year, User loggedUser)
        {
            if(loggedUser.Admin)
            {
                return _articleRepository.GetAll(m => m.DateCreated.Year == year)
                                         .GroupBy(m => m.DateCreated.Month)
                                         .Select(m => m.Count());
            } else
            {
                throw new UnauthorizedAccessException("Se necesitan permisos de administrador");
            }
        }

        public Article UpdateArticle(int articleId, Article anArticle, User loggedUser)
        {
            Article article = _articleRepository.Get(ArticleById(articleId, loggedUser));
            isValidArticle(anArticle);
            if (loggedUser.Id == article.UserId)
            {
                article.Name = anArticle.Name;
                article.Body = anArticle.Body;
                article.Private = anArticle.Private;
                article.DateModified = DateTime.Now;
                article.Template = anArticle.Template;
                article.Image = anArticle.Image;
                this._articleRepository.Update(article);
                return article;
            } else
            {
                throw new UnauthorizedAccessException("Sólo el creador del artículo puede modificarlo");
            };
        }

        private Func<Article, bool> ArticleById(int id, User loggedUser)
        {
            return a => a.Id == id && a.DateDeleted == null && (!a.Private || a.UserId == loggedUser.Id);
        }

        private Func<Article, bool> ArticleByTextSearch(string text, User loggedUser)
        {
            return article => article.DateDeleted == null &&
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

