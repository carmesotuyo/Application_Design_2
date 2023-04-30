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

namespace WebApi.Test
{
	[TestClass]
	public class ArticleControllerTest
    {
        private Mock<IArticleLogic> articleLogicMock;
        private ArticleController controller;

        private static readonly Article article = new Article()
        {
            // article data I may need
        };

        [TestInitialize]
		public void InitTest()
        {
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Strict);
            controller = new ArticleController(articleLogicMock.Object);

            //httpContext = new DefaultHttpContext();
            //httpContext.Items["user"] = User;

            //ControllerContext controllerContext = new ControllerContext()
            //{
            //    HttpContext = httpContext
            //};
            //controller = new InviteController(anInviteLogicMock.Object)
            //{
            //    ControllerContext = controllerContext
            //};
        }
        [TestMethod]
        public void GetArticlesById()
        {
            articleLogicMock.Setup(m => m.GetArticleById(It.IsAny<int>())).Returns(article);

            IActionResult resultCall = controller.GetArticleById(article.Id);
            articleLogicMock.VerifyAll();
            OkObjectResult result = resultCall as OkObjectResult;

            Assert.IsNotNull(result);

            //InviteOutModel resultInvite = result.Value as InviteOutModel;

            //Assert.IsNotNull(resultInvite);

            Assert.IsTrue(result.Value.Equals(article));
        }
	}
}

