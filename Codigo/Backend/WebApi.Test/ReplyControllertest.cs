using System;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Domain.Entities;

namespace WebApi.Test
{
	[TestClass]
	public class ReplyControllertest
    {
        private Mock<IReplyLogic> replyLogicMock;
        private ReplyController controller;
        HttpContext httpContext;
        private User user;
        private Reply reply;

        [TestInitialize]
        public void InitTest()
        {
            replyLogicMock = new Mock<IReplyLogic>(MockBehavior.Strict);
            controller = new ReplyController(replyLogicMock.Object);
            user = new User();
            reply = new Reply();

            httpContext = new DefaultHttpContext();
            httpContext.Items["user"] = user;

            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            controller = new ReplyController(replyLogicMock.Object)
            {
                ControllerContext = controllerContext
            };
        }


        [TestMethod]
        public void PostReplyOk()
        {
            replyLogicMock!.Setup(m => m.CreateReply(It.IsAny<Reply>(), It.IsAny<User>())).Returns(reply);

            var result = controller!.PostReply(reply);
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            replyLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult.Value, reply);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void PostReplyWithoutPermissions()
        {
            replyLogicMock.Setup(m => m.CreateReply(It.IsAny<Reply>(), It.IsAny<User>())).Throws(new UnauthorizedAccessException());

            var result = controller.PostReply(It.IsAny<Reply>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            replyLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }
    }
}

