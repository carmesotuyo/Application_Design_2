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

namespace WebApi.Test
{
    [TestClass]
    public class SessionControllerTest
	{

        private Mock<ISessionLogic> sessionLogicMock;
        private SessionController controller;
        //HttpContext httpContext;

        private Session session;
        private string username;
        private string password;
        private LoginRequestDTO credentials;
        private Guid token;
        private User user;
        private Comment comment;
        private List<Comment> comments;
        private LoginResponseDTO responseDTO;

        [TestInitialize]
        public void InitTest()
        {
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            controller = new SessionController(sessionLogicMock.Object);

            session = new Session() { Id = 1 };
            username = "username";
            password = "password";
            credentials = new LoginRequestDTO(username, password);
            token = Guid.NewGuid();
            user = new User();
            comment = new Comment();
            comments = new List<Comment>() { comment };
            responseDTO = new LoginResponseDTO(token, comments);

            //httpContext = new DefaultHttpContext();
            //httpContext.Items["user"] = user;

            //ControllerContext controllerContext = new ControllerContext()
            //{
            //    HttpContext = httpContext
            //};
            //controller = new SessionController(sessionLogicMock.Object)
            //{
            //    ControllerContext = controllerContext
            //};
        }

        [TestMethod]
        public void LoginOk()
        {
            sessionLogicMock!.Setup(m => m.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(token);
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);
            sessionLogicMock!.Setup(m => m.GetCommentsWhileLoggedOut(It.IsAny<int>())).Returns(comments);

            var result = controller!.Login(credentials);
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;
            var receivedDTO = objectResult.Value as LoginResponseDTO;

            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(receivedDTO.Token, token);
            Assert.AreEqual(receivedDTO.Comments, comments);
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
            sessionLogicMock!.Setup(m => m.Logout(It.IsAny<int>(), It.IsAny<User>()));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            var result = controller!.Logout(session.Id, token.ToString());
            var objectResult = result as OkResult;
            var statusCode = objectResult?.StatusCode;

            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(BadHttpRequestException))]
        public void LogoutBadRequest()
        {
            sessionLogicMock!.Setup(m => m.Logout(It.IsAny<int>(), It.IsAny<User>())).Throws(new BadHttpRequestException("Incorrect request to Logout", 400));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            var result = controller!.Logout(session.Id, token.ToString());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            sessionLogicMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }
    }
}

