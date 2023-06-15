using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.Domain.Entities;
using System.Data;
using BlogsApp.WebAPI.Controllers;
using System.Net.Http;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.Logging.Logic.Services;
using BlogsApp.WebAPI.DTOs;

namespace WebApi.Test
{
	[TestClass]
	public class ArticleControllerTest
    {
        private Mock<IArticleLogic> articleLogicMock;
        private Mock<ISessionLogic> sessionLogicMock;
        private ArticleController controller;
        private Mock<ILoggerService> loggerLogicMock;

        private Article article;
        private IEnumerable<Article> articles;
        private User userBlogger;
        private User userAdmin;
        private Dictionary<string, int> yearlyStats;
        private Guid token;
        private BasicArticleDto basicArticleDto;
        private UpdateArticleRequestDTO updateArticleDto;
        private IEnumerable<BasicArticleDto> responseList;

        [TestInitialize]
		public void InitTest()
        {
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Strict);
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            loggerLogicMock = new Mock<ILoggerService>(MockBehavior.Strict);
            controller = new ArticleController(articleLogicMock.Object, sessionLogicMock.Object, loggerLogicMock.Object);
            userBlogger = new User() { Blogger = true, Id = 1, Username = "blogger" };
            article = new Article() { Id = 1, Name = "name", UserId = 1, User = userBlogger, Body = "body", Private = false, Template = 1, Image = "" };
            articles = new List<Article>() { article };
            userAdmin = new User() { Admin = true };
            yearlyStats = new Dictionary<string, int>();
            token = Guid.NewGuid();
            basicArticleDto = ArticleConverter.ToDto(article);
            updateArticleDto = new UpdateArticleRequestDTO() { Body = "updated body" };
            responseList = ArticleConverter.ToDtoList(articles);
        }

        [TestMethod]
        public void GetAllArticles()
        {
            articleLogicMock.Setup(m => m.GetArticles(It.IsAny<User>(), null)).Returns(articles);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller!.Get(default, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetArticlesById()
        {
            articleLogicMock.Setup(m => m.GetArticleById(It.IsAny<int>(), It.IsAny<User>())).Returns(article);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller!.GetArticleById(article.Id, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetArticlesBySearch()
        {
            articleLogicMock.Setup(m => m.GetArticles(It.IsAny<User>(), It.IsAny<string>())).Returns(articles);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);
            loggerLogicMock.Setup(m => m.LogSearch(It.IsAny<int>(), It.IsAny<string>()));

            var result = controller!.Get("search text", token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;


            articleLogicMock.VerifyAll();
            loggerLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetArticlesStats()
        {
            articleLogicMock.Setup(m => m.GetStatsByYear(It.IsAny<int>(), It.IsAny<User>())).Returns(yearlyStats);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller!.GetStatsByYear(2020, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
            Assert.IsTrue(objectResult.Value.Equals(yearlyStats));
        }

        [TestMethod]
        public void DeleteArticleOk()
        {
            articleLogicMock!.Setup(m => m.DeleteArticle(article.Id, userBlogger));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller!.DeleteArticle(article.Id, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteArticleWithoutPermissions()
        {
            articleLogicMock.Setup(m => m.DeleteArticle(It.IsAny<int>(), It.IsAny<User>())).Throws(new UnauthorizedAccessException());
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller.DeleteArticle(It.IsAny<int>(), token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void DeleteArticleNotFound()
        {
            articleLogicMock.Setup(m => m.DeleteArticle(It.IsAny<int>(), It.IsAny<User>())).Throws(new NotFoundDbException(""));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller.DeleteArticle(It.IsAny<int>(), token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        public void PostArticleOk()
        {
            articleLogicMock!.Setup(m => m.CreateArticle(It.IsAny<Article>(), It.IsAny<User>())).Returns(article);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller!.PostArticle(basicArticleDto, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void PostArticleWithoutPermissions()
        {
            articleLogicMock.Setup(m => m.CreateArticle(It.IsAny<Article>(), It.IsAny<User>())).Throws(new UnauthorizedAccessException());
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);

            var result = controller.PostArticle(basicArticleDto, token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }

        [TestMethod]
        public void UpdateArticleOk()
        {
            articleLogicMock!.Setup(x => x.UpdateArticle(It.IsAny<int>(), It.IsAny<Article>(), It.IsAny<User>())).Returns(article);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);
            articleLogicMock!.Setup(x => x.GetArticleById(It.IsAny<int>(), It.IsAny<User>())).Returns(article);

            var result = controller!.UpdateArticle(article!.Id, updateArticleDto, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void UpdateArticleWithoutPermissions()
        {
            articleLogicMock!.Setup(x => x.UpdateArticle(article.Id, article!, userBlogger)).Throws(new UnauthorizedAccessException());
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);
            articleLogicMock!.Setup(x => x.GetArticleById(It.IsAny<int>(), It.IsAny<User>())).Returns(article);

            var result = controller!.UpdateArticle(article!.Id, updateArticleDto, token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void UpdateArticleNotFound()
        {
            articleLogicMock!.Setup(x => x.UpdateArticle(article.Id, article!, userBlogger)).Throws(new NotFoundDbException(""));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(userBlogger);
            articleLogicMock!.Setup(x => x.GetArticleById(It.IsAny<int>(), It.IsAny<User>())).Returns(article);

            var result = controller!.UpdateArticle(article!.Id, updateArticleDto, token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }
    }
}

