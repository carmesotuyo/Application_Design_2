using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Domain.Exceptions;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.Logging.Logic.Services;

namespace WebApi.Test
{
    [TestClass]
    public class SessionControllerTest
	{
        private Mock<ISessionLogic> sessionLogicMock;
        private SessionController controller;

        private Session session;
        private string username;
        private string password;
        private LoginRequestDTO credentials;
        private Guid token;
        private User user;
        private Comment comment;
        private List<Comment> comments;
        private NotificationCommentDto notifiedComment;
        private List<NotificationCommentDto> notifiedComments;
        private LoginResponseDTO responseDTO;
        private Mock<ILoggerService> loggerLogicMock;

        [TestInitialize]
        public void InitTest()
        {
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            loggerLogicMock = new Mock<ILoggerService>(MockBehavior.Strict);
            controller = new SessionController(sessionLogicMock.Object, loggerLogicMock.Object);
            

            session = new Session() { Id = 1 };
            username = "username";
            password = "password";
            credentials = new LoginRequestDTO(username, password);
            token = Guid.NewGuid();
            user = new User();
            comment = new Comment() { Id = 1, Body = "body", Article = new Article() { Id = 1 } };
            comments = new List<Comment>() { comment };
            notifiedComment = CommentConverter.toNotificationDto(comment);
            notifiedComments = new List<NotificationCommentDto>() { notifiedComment };
            responseDTO = new LoginResponseDTO(token, notifiedComments);
        }

        [TestMethod]
        public void LoginOk()
        {
            sessionLogicMock!.Setup(m => m.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(token);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);
            sessionLogicMock!.Setup(m => m.GetCommentsWhileLoggedOut(It.IsAny<User>())).Returns(comments);
            loggerLogicMock.Setup(m => m.LogLogin(It.IsAny<int>()));

            var result = controller!.Login(credentials);
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;
            var receivedDTO = objectResult.Value as LoginResponseDTO;

            sessionLogicMock.VerifyAll();
            loggerLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(receivedDTO.Token, token);
            Assert.AreEqual(receivedDTO.Comments.Count(), notifiedComments.Count());
            Assert.AreEqual(receivedDTO.Comments.First().CommentId, notifiedComments.First().CommentId);
        }

        [TestMethod]
        [ExpectedException(typeof(BadInputException))]
        public void LoginIncorrectCredentials()
        {
            sessionLogicMock!.Setup(m => m.Login(It.IsAny<string>(), It.IsAny<string>())).Throws(new BadInputException("Incorrect credentials"));

            var result = controller!.Login(credentials);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            sessionLogicMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }


        [TestMethod]
        public void LogoutOk()
        {
            sessionLogicMock!.Setup(m => m.Logout(It.IsAny<User>()));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            var result = controller!.Logout(token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(BadHttpRequestException))]
        public void LogoutBadRequest()
        {
            sessionLogicMock!.Setup(m => m.Logout(It.IsAny<User>())).Throws(new BadHttpRequestException("Incorrect request to Logout", 400));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            var result = controller!.Logout(token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            sessionLogicMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }
    }
}

