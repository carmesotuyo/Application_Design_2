﻿using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using System.Data;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace BlogsApp.BusinessLogic.Logics
{
    public class ArticleLogic : IArticleLogic
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleLogic(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public Article CreateArticle(Article article, User loggedUser)
        {
            if (loggedUser.Blogger && isValidArticle(article))
            {
                this._articleRepository.Add(article);
                
                return article;
            }

            throw new UnauthorizedAccessException("Solo los Bloggers pueden crear articulos.");
        }

        public void DeleteArticle(int articleId, User loggedUser)
        {
            Article article = _articleRepository.Get(ArticleById(articleId));
            if(loggedUser.Id == article.UserId)
            {
                article.DateDeleted = DateTime.Now;
                this._articleRepository.Update(article);
            } else
            {
                throw new UnauthorizedAccessException("Sólo el creador del artículo puede modificarlo")
            }
        }

        public Article GetArticleById(int id)
        {
            return _articleRepository.Get(ArticleById(id));
        }

        public IEnumerable<Article> GetArticles(string? searchText)
        {
            if (searchText == null)
            {
                return _articleRepository.GetAll(m => m.DateDeleted == null)
                                 .OrderByDescending(m => m.DateModified)
                                 .Take(10);
            }
            else
            {
                return _articleRepository.GetAll(ArticleByTextSearch(searchText));
            }
        }

        public IEnumerable<Article> GetArticlesByUser(int userId)
        {
            return _articleRepository.GetAll(m => m.DateDeleted == null && m.UserId == userId);
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
            Article article = _articleRepository.Get(ArticleById(articleId));
            article.Name = anArticle.Name;
            article.Body = anArticle.Body;
            article.Private = anArticle.Private;
            article.DateModified = DateTime.Now;
            article.Template = anArticle.Template;
            article.Image = anArticle.Image;
            this._articleRepository.Update(article);
            return article;
        }

        private Func<Article, bool> ArticleById(int id)
        {
            return a => a.Id == id && a.DateDeleted != null;
        }

        private Func<Article, bool> ArticleByTextSearch(string text)
        {
            return article => article.DateDeleted == null &&
                              (article.Name.Contains(text) || article.Body.Contains(text));
        }

        public bool isValidArticle(Article? article)
        {
            if (article == null)
            {
                return false;
                //throw new BusinessLogicException("Articulo inválido");
            }
            if (article.Name == null || article.Name == "")
            {
                //throw new BusinessLogicException("Debe ingresar un nombre");
            }
            if (article.Body == null || article.Body == "")
            {
                //throw new BusinessLogicException("Debe ingresar una contenido");
            }
            if (article.User == null)
            {
                //throw new BusinessLogicException("El articulo debe contener un autor");
            }
            if (article.Template == null)
            {
                //throw new BusinessLogicException("Debe ingresar un template válido");
            }
            return true;
        }



    }
}

