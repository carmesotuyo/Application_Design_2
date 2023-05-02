using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.Domain.Entities;
using System.Data;
using Microsoft.AspNetCore.Http;
using BlogsApp.WebAPI.Controllers;
using BlogsApp.WebAPI.DTOs;
using System.Net.Http;
using BlogsApp.DataAccess.Interfaces.Exceptions;

namespace WebApi.Test
{
	[TestClass]
	public class ArticleControllerTest
    {
        private Mock<IArticleLogic> articleLogicMock;
        private ArticleController controller;
        HttpContext httpContext;

        private Article article;
        private IEnumerable<Article> articles;
        private StatsModel yearlyStats;
        private User userBlogger;
        private User userAdmin;

        [TestInitialize]
		public void InitTest()
        {
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Strict);
            controller = new ArticleController(articleLogicMock.Object);
            userBlogger = new User() { Blogger = true, Id = 1 };
            article = new Article() { Id = 1, UserId = 1 };
            articles = new List<Article>() { article };
            yearlyStats = new StatsModel();
            userAdmin = new User() { Admin = true };

            httpContext = new DefaultHttpContext();
            httpContext.Items["user"] = userBlogger;

            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            controller = new ArticleController(articleLogicMock.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [TestMethod]
        public void GetAllArticles()
        {
            // si le paso default falla, pero si le paso It.IsAny<string>() no, deberia poder ser opcional el campo
            // casi de arriba era cuando tenia definico string? search = "" tanto en IArticleLogic como en ArticleController
            // ahora le saque el "" y me anda con el default, A CHEQUEAR
            articleLogicMock.Setup(m => m.GetArticles(userBlogger, null)).Returns(articles);

            IActionResult result = controller!.Get(default);
            articleLogicMock.VerifyAll();
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.IsNotNull(result);
            articleLogicMock.VerifyAll();

            Assert.IsTrue(objectResult.Value.Equals(articles));
        }

        [TestMethod]
        public void GetArticlesById()
        {
            articleLogicMock.Setup(m => m.GetArticleById(It.IsAny<int>())).Returns(article);

            IActionResult result = controller!.GetArticleById(article.Id);
            articleLogicMock.VerifyAll();
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.IsNotNull(result);
            articleLogicMock.VerifyAll();

            Assert.IsTrue(objectResult.Value.Equals(article));
        }

        [TestMethod]
        public void GetArticlesBySearch()
        {
            articleLogicMock.Setup(m => m.GetArticles(It.IsAny<User>(), It.IsAny<string>())).Returns(articles);

            IActionResult result = controller!.Get("search text");
            articleLogicMock.VerifyAll();
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.IsNotNull(result);
            articleLogicMock.VerifyAll();

            Assert.IsTrue(objectResult.Value.Equals(articles));
        }

        [TestMethod]
        public void GetArticlesStats()
        {
            httpContext.Items["user"] = userAdmin;

            articleLogicMock.Setup(m => m.GetStatsByYear(It.IsAny<int>(), It.IsAny<User>())).Returns(yearlyStats.stats);

            IActionResult result = controller!.GetStatsByYear(2020);
            articleLogicMock.VerifyAll();

            OkObjectResult objectResult = result as OkObjectResult;

            Assert.IsNotNull(result);
            articleLogicMock.VerifyAll();

            Assert.IsTrue(objectResult.Value.Equals(yearlyStats.stats));
        }

        [TestMethod]
        public void DeleteArticleOk()
        {
            articleLogicMock!.Setup(m => m.DeleteArticle(article.Id, userBlogger));

            var result = controller!.DeleteArticle(article.Id);
            var objectResult = result as OkResult;
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

            var result = controller.DeleteArticle(It.IsAny<int>());
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

            var result = controller.DeleteArticle(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        public void PostArticleOk()
        {
            articleLogicMock!.Setup(m => m.CreateArticle(It.IsAny<Article>(), It.IsAny<User>())).Returns(article);

            var result = controller!.PostArticle(article);
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult.Value, article);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void PostArticleWithoutPermissions()
        {
            articleLogicMock.Setup(m => m.CreateArticle(It.IsAny<Article>(), It.IsAny<User>())).Throws(new UnauthorizedAccessException());

            var result = controller.PostArticle(It.IsAny<Article>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }

        [TestMethod]
        public void UpdateArticleOk()
        {
            articleLogicMock!.Setup(x => x.UpdateArticle(article.Id, article!, userBlogger)).Returns(It.IsAny<Article>());
            var result = controller!.UpdateArticle(article!.Id, article);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void UpdateArticleWithoutPermissions()
        {
            articleLogicMock!.Setup(x => x.UpdateArticle(article.Id, article!, userBlogger)).Throws(new UnauthorizedAccessException());
            var result = controller!.UpdateArticle(article!.Id, article);
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
            var result = controller!.UpdateArticle(article!.Id, article);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }
    }
}

