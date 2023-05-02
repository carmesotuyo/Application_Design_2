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
        private Mock<ISessionLogic> sessionLogicMock;
        private CommmentController controller;
        //HttpContext httpContext;

        private Comment comment;
        private User user;
        private Guid token;

        [TestInitialize]
        public void InitTest()
        {
            commentLogicMock = new Mock<ICommentLogic>(MockBehavior.Strict);
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            controller = new CommmentController(commentLogicMock.Object, sessionLogicMock.Object);

            comment = new Comment();
            user = new User();
            token = Guid.NewGuid();

            //httpContext = new DefaultHttpContext();
            //httpContext.Items["user"] = user;

            //ControllerContext controllerContext = new ControllerContext()
            //{
            //    HttpContext = httpContext
            //};
            //controller = new CommmentController(commentLogicMock.Object)
            //{
            //    ControllerContext = controllerContext
            //};
        }

        [TestMethod]
		public void CreateComment()
		{
            commentLogicMock.Setup(m => m.CreateComment(It.IsAny<Comment>(), It.IsAny<User>())).Returns(comment);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            var result = controller.CreateComment(comment, token.ToString());

            commentLogicMock.VerifyAll();
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.IsNotNull(objectResult);

            Assert.IsTrue(objectResult.Value.Equals(comment));
        }
	}
}

