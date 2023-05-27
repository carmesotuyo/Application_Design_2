using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.Controllers;
using BlogsApp.WebAPI.DTOs;

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
        private Comment parentComment;
        private BasicCommentDTO commentDTO;
        private User user;
        private Guid token;
        private Article article;

        [TestInitialize]
        public void InitTest()
        {
            commentLogicMock = new Mock<ICommentLogic>(MockBehavior.Strict);
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Strict);
            controller = new CommmentController(commentLogicMock.Object, sessionLogicMock.Object, articleLogicMock.Object);

            article = new Article() { Id = 1 };
            comment = new Comment() { Body = "Body", Article = article };
            parentComment = new Comment() { Body = "Parent", Article = article, Id = 1 };
            commentDTO = CommentConverter.toBasicDto(comment);
            user = new User();
            token = Guid.NewGuid();
        }

        [TestMethod]
		public void CreateComment()
		{
            commentLogicMock.Setup(m => m.CreateComment(It.IsAny<Comment>(), It.IsAny<User>())).Returns(comment);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);
            articleLogicMock!.Setup(m => m.GetArticleById(It.IsAny<int>(), It.IsAny<User>())).Returns(article);

            var result = controller.CreateComment(commentDTO, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            commentLogicMock.VerifyAll();
            articleLogicMock.VerifyAll();
            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void PostCommentWithoutPermissions()
        {
            commentLogicMock.Setup(m => m.CreateComment(It.IsAny<Comment>(), It.IsAny<User>())).Throws(new UnauthorizedAccessException());
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);
            articleLogicMock!.Setup(m => m.GetArticleById(It.IsAny<int>(), It.IsAny<User>())).Returns(article);

            var result = controller.CreateComment(commentDTO, token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            commentLogicMock.VerifyAll();
            articleLogicMock.VerifyAll();
            sessionLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }

        [TestMethod]
        public void CreateSubCommentFromParent()
        {
            commentLogicMock.Setup(m => m.ReplyToComment(It.IsAny<Comment>(), It.IsAny<Comment>(), It.IsAny<User>())).Returns(comment);
            commentLogicMock.Setup(m => m.GetCommentById(It.IsAny<int>())).Returns(parentComment);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);
            articleLogicMock!.Setup(m => m.GetArticleById(It.IsAny<int>(), It.IsAny<User>())).Returns(article);

            var result = controller.CreateSubCommentFromParent(commentDTO, parentComment.Id, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            commentLogicMock.VerifyAll();
            articleLogicMock.VerifyAll();
            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }
    }
}

