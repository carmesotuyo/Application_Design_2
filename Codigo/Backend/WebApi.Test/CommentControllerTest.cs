using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;

namespace WebApi.Test
{
	[TestClass]
	public class CommentControllerTest
    {
        Mock<ICommentLogic> commentLogicMock;
        private CommmentController controller;
        HttpContext httpContext;

        static readonly Comment comment = new Comment() { };
        static readonly User user = new User() { };

        [TestInitialize]
        public void InitTest()
        {
            commentLogicMock = new Mock<ICommentLogic>(MockBehavior.Strict);

            httpContext = new DefaultHttpContext();
            httpContext.Items["user"] = user;

            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            controller = new CommmentController(commentLogicMock.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [TestMethod]
		public void CreateComment()
		{
            commentLogicMock.Setup(m => m.CreateComment(comment, user)).Returns(comment);

            var result = controller.CreateComment(comment);

            commentLogicMock.VerifyAll();
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.IsNotNull(objectResult);

            Assert.IsTrue(objectResult.Value.Equals(comment));
        }
	}
}

