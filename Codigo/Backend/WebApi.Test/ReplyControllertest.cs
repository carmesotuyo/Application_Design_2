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
        private Mock<ISessionLogic> sessionLogicMock;
        private ReplyController controller;
        private User user;
        private Reply reply;
        private Guid token;

        [TestInitialize]
        public void InitTest()
        {
            replyLogicMock = new Mock<IReplyLogic>(MockBehavior.Strict);
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            controller = new ReplyController(replyLogicMock.Object, sessionLogicMock.Object);
            user = new User();
            reply = new Reply();
            token = Guid.NewGuid();
        }


        [TestMethod]
        public void PostReplyOk()
        {
            replyLogicMock!.Setup(m => m.CreateReply(It.IsAny<Reply>(), It.IsAny<User>())).Returns(reply);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            var result = controller!.PostReply(reply, token.ToString());
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
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            var result = controller.PostReply(It.IsAny<Reply>(), token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            replyLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }
    }
}

