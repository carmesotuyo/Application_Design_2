using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Controllers;
using BlogsApp.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Domain.Exceptions;

namespace WebApi.Test
{
    [TestClass]
    public class SessionControllerTest
	{

        private Mock<ISessionLogic> sessionLogicMock;
        private SessionController controller;
        //HttpContext httpContext;

        //private Session session;
        private string username;
        private string password;
        private Guid token;

        //private Article article;

        [TestInitialize]
        public void InitTest()
        {
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            controller = new SessionController(sessionLogicMock.Object);

            //session = new Session();
            username = "username";
            password = "password";
            token = Guid.NewGuid();

            //httpContext = new DefaultHttpContext();
            //httpContext.Items["user"] = userBlogger;

            //ControllerContext controllerContext = new ControllerContext()
            //{
            //    HttpContext = httpContext
            //};
            //controller = new ArticleController(articleLogicMock.Object)
            //{
            //    ControllerContext = controllerContext
            //};
        }


        [TestMethod]
        public void LoginOk()
        {
            sessionLogicMock!.Setup(m => m.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(token);

            var result = controller!.Login(username, password);
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult.Value, token);
        }

        [TestMethod]
        [ExpectedException(typeof(BadInputException))]
        public void LoginIncorrectCredentials()
        {
            sessionLogicMock!.Setup(m => m.Login(It.IsAny<string>(), It.IsAny<string>())).Throws(new BadInputException("Incorrect credentials"));

            var result = controller!.Login(username, password);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            sessionLogicMock.VerifyAll();
            Assert.AreEqual(objectResult.Value, token);
            Assert.AreEqual(400, statusCode);
        }
    }
}

