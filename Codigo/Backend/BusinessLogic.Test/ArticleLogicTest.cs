﻿using System.Collections.Generic;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Domain;
using System.Linq.Expressions;

namespace BlogsApp.BusinessLogic.Tests
{
    [TestClass]
    public class ArticleLogicTests
    {
        private static readonly User user = new User { Id = 1, Username = "TestUserName", Name = "Test User", Blogger = true };
        private readonly Article Articulo1 = new Article { Id = 1, Name = "Test Article, text1", DateModified = DateTime.Today, DateCreated = DateTime.Parse("2022-01-05") };

        private readonly Article Articulo2 = new Article { Id = 2, Name = "Test Article 2", Body = "text1", DateModified = DateTime.Today, DateCreated = DateTime.Parse("2022-01-05") };

        private readonly Article Articulo3 = new Article { Id = 3, Name = "Test Article 3", DateModified = DateTime.Today, User = user, DateCreated = DateTime.Parse("2022-01-05") };

        private readonly Article Articulo4 = new Article { Id = 4, Name = "Test Article 4", DateModified = DateTime.Today, User = user , DateCreated = DateTime.Parse("2022-02-05") };

        private readonly Article Articulo5 = new Article { Id = 5, Name = "Test Article 5", DateModified = DateTime.Today   , DateCreated = DateTime.Parse("2022-02-05") };

        private readonly Article Articulo6 = new Article { Id = 6, Name = "Test Article 6", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-03-05") };

        private readonly Article Articulo7 = new Article { Id = 7, Name = "Test Article 7", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-04-05") };

        private readonly Article Articulo8 = new Article { Id = 8, Name = "Test Article 8", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-05-05") };

        private readonly Article Articulo9 = new Article { Id = 9, Name = "Test Article 9", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-05-05") };

        private readonly Article Articulo10 = new Article { Id = 10, Name = "Test Article 10", DateCreated = DateTime.Parse("2022-06-05") };

        private readonly Article Articulo11 = new Article { Id = 11, Name = "Test Article 11", DateCreated = DateTime.Parse("2022-07-05"), User = user };

        private readonly Article Articulo12 = new Article { Id = 12, Name = "Test Article 12", Body = "Text1" , DateCreated = DateTime.Parse("2022-07-05") };


        private Mock<IArticleRepository> articleRepository;
        private IArticleLogic articleLogic;
        private ICollection<Article> allArticles;
        private ICollection<Article> newArticle;

        [TestInitialize]
        public void TestInitialize()
        {
            articleRepository = new Mock<IArticleRepository>(MockBehavior.Default);
            articleLogic = new ArticleLogic(articleRepository.Object);
            allArticles = new List<Article>() { Articulo1, Articulo2, Articulo3, Articulo4, Articulo5, Articulo6, Articulo7, Articulo8, Articulo9, Articulo10, Articulo11, Articulo12 };
            newArticle = new List<Article>() { Articulo11 };
        }

        [TestMethod]
        public void GetArticleByIdTest()
        {
            articleRepository.Setup(x => x.Get(It.IsAny<Func<Article, bool>>())).Returns(Articulo1);
            Article result = articleLogic.GetArticleById(1);
            articleRepository.VerifyAll();
            Assert.IsTrue(result.Id == 1);
        }

        [TestMethod]
        public void GetLastArticlesTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);

            IEnumerable<Article> result = articleLogic.GetArticles(null);

            articleRepository.VerifyAll();
            Assert.IsTrue(result.Count() == 10);
        }


        [TestMethod]
        public void GetArticles_SearchTextIsNull_ReturnsLastTenArticles()
        {
            var articles = new List<Article>();
            for (int i = 0; i < 15; i++)
            {
                articles.Add(new Article
                {
                    Id = i + 1,
                    Name = $"Article {i + 1}",
                    Body = $"Article {i + 1} body",
                    DateModified = DateTime.Now.AddDays(-i),
                    DateDeleted = null
                });
            }

            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(articles);

            var result = articleLogic.GetArticles(null);

            Assert.AreEqual(10, result.Count());
            Assert.AreEqual(articles[0].Id, result.ElementAt(0).Id);
            Assert.AreEqual(articles[1].Id, result.ElementAt(1).Id);
            Assert.AreEqual(articles[2].Id, result.ElementAt(2).Id);
            Assert.AreEqual(articles[3].Id, result.ElementAt(3).Id);
            Assert.AreEqual(articles[4].Id, result.ElementAt(4).Id);
            Assert.AreEqual(articles[5].Id, result.ElementAt(5).Id);
            Assert.AreEqual(articles[6].Id, result.ElementAt(6).Id);
            Assert.AreEqual(articles[7].Id, result.ElementAt(7).Id);
            Assert.AreEqual(articles[8].Id, result.ElementAt(8).Id);
            Assert.AreEqual(articles[9].Id, result.ElementAt(9).Id);
        }

        [TestMethod]
        public void GetArticles_SearchTextIsNotNull_ReturnsArticlesContainingSearchText()
        {
            var articles = new List<Article>
        {
            new Article { Id = 1, Name = "Article 1", Body = "Article 1 body", DateDeleted = null },
            new Article { Id = 2, Name = "Article 2, textSearch", Body = "Article 2 body", DateDeleted = null },
            new Article { Id = 3, Name = "Article 3", Body = "textSearch", DateDeleted = null },
            new Article { Id = 4, Name = "Article 4", Body = "Article 4 body", DateDeleted = null },
            new Article { Id = 5, Name = "Article 5", Body = "Article 5 body", DateDeleted = null },
        };

            articleRepository.Setup(r => r.GetAll(It.IsAny<Func<Article, bool>>())).Returns(articles);

            var result = articleLogic.GetArticles("textSearch");

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(2, result.ElementAt(0).Id);
            Assert.AreEqual(3, result.ElementAt(1).Id);
        }

        [TestMethod]
        public void GetArticlesByUserTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);

            IEnumerable<Article> result = articleLogic.GetArticlesByUser(user.Id);

            articleRepository.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(allArticles));
        }

        [TestMethod]
        public void GetStatsByYearTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);

            IEnumerable<int> result = articleLogic.GetStatsByYear(2022);

            articleRepository.VerifyAll();
            Assert.IsTrue(result.Count() == 7);
        }

        [TestMethod]
        public void CreateArticleTest()
        {

            articleRepository.Setup(x => x.Add(It.IsAny<Article>())).Returns(Articulo11);

            Article result = articleLogic.CreateArticle(Articulo11, user);

            articleRepository.VerifyAll();
            Assert.AreEqual(result, Articulo11);
        }

        [TestMethod]
        public void SetsDateDeletedTest()
        {
            articleRepository.Setup(r => r.Get(It.IsAny<Func<Article, bool>>())).Returns(Articulo11);

            articleLogic.DeleteArticle(Articulo11.Id);
            Article result = Articulo11;

            articleRepository.Verify(r => r.Update(It.IsAny<Article>()), Times.Once);
            Assert.IsTrue(Articulo11.DateDeleted != null);

            Assert.IsNotNull(Articulo11.DateDeleted);
        }

        [TestMethod]
        public void UpdateArticleNameTest()
        {
            int articleId = 1;
            Article existingArticle = new Article { Id = articleId, Name = "Old name" };
            Article updatedArticle = new Article { Name = "New name"};
            articleRepository.Setup(repo => repo.Get(It.IsAny<Func<Article, bool>>())).Returns(existingArticle);

            Article result = articleLogic.UpdateArticle(articleId, updatedArticle);

            articleRepository.Verify(repo => repo.Update(existingArticle), Times.Once);
            Assert.AreEqual(updatedArticle.Name, result.Name);
            
        }

        [TestMethod]
        public void UpdateArticleBodyTest()
        {
            int articleId = 1;
            Article existingArticle = new Article { Id = articleId, Body = "Old body" };
            Article updatedArticle = new Article { Body = "New body" };
            articleRepository.Setup(repo => repo.Get(It.IsAny<Func<Article, bool>>())).Returns(existingArticle);

            Article result = articleLogic.UpdateArticle(articleId, updatedArticle);

            articleRepository.Verify(repo => repo.Update(existingArticle), Times.Once);
            Assert.AreEqual(updatedArticle.Body, result.Body);
        }

        [TestMethod]
        public void UpdateArticleImageTest()
        {
            int articleId = 1;
            Article existingArticle = new Article { Id = articleId, Image = "OldImage.jpeg" };
            Article updatedArticle = new Article { Image = "NewImage.jpeg" };
            articleRepository.Setup(repo => repo.Get(It.IsAny<Func<Article, bool>>())).Returns(existingArticle);

            Article result = articleLogic.UpdateArticle(articleId, updatedArticle);

            articleRepository.Verify(repo => repo.Update(existingArticle), Times.Once);
            Assert.AreEqual(updatedArticle.Image, result.Image);
        }

        [TestMethod]
        public void UpdateArticleDateModifiedTest()
        {
            int articleId = 1;
            Article existingArticle = new Article { Id = articleId, Image = "OldImage.jpeg", DateModified = DateTime.Parse("2022-01-05") };
            Article updatedArticle = new Article { Image = "NewImage.jpeg" };
            articleRepository.Setup(repo => repo.Get(It.IsAny<Func<Article, bool>>())).Returns(existingArticle);

            Article result = articleLogic.UpdateArticle(articleId, updatedArticle);

            articleRepository.Verify(repo => repo.Update(existingArticle), Times.Once);
            Assert.AreNotEqual(updatedArticle.DateModified, result.DateModified);
        }

    }
}