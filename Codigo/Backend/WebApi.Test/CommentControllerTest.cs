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
        private Mock<IArticleLogic> articleLogicMock;
        private CommmentController controller;

        private Comment comment;
        private User user;
        private Guid token;

        [TestInitialize]
        public void InitTest()
        {
            commentLogicMock = new Mock<ICommentLogic>(MockBehavior.Strict);
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Strict);
            controller = new CommmentController(commentLogicMock.Object, sessionLogicMock.Object, articleLogicMock.Object);

            comment = new Comment();
            user = new User();
            token = Guid.NewGuid();
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

