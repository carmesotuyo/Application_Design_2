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
using BlogsApp.WebAPI.Models;
using System.Net.Http;

namespace WebApi.Test
{
	[TestClass]
	public class ArticleControllerTest
    {
        private Mock<IArticleLogic> articleLogicMock;
        private ArticleController controller;
        HttpContext httpContext;

        private static readonly Article article = new Article() { };
        private IEnumerable<Article> articles;
        private StatsModel yearlyStats;
        private User userBlogger;
        private User userAdmin;

        [TestInitialize]
		public void InitTest()
        {
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Strict);
            controller = new ArticleController(articleLogicMock.Object);
            articles = new List<Article>() { article };
            yearlyStats = new StatsModel();
            userBlogger = new User() { Blogger = true };
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
	}
}

