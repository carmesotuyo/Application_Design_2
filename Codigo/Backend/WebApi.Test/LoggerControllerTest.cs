using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Logging.Logic.Services;
using BlogsApp.Logging.Entities;

namespace WebApi.Test
{
    [TestClass]
    public class LoggerControlerTest
    {
        [TestClass]
        public class LogControllerTests
        {
            private LogController _logController;
            private Mock<ILoggerService> _loggerServiceMock;
            HttpContext httpContext;
            private User user;

            [TestInitialize]
            public void TestInitialize()
            {
                _loggerServiceMock = new Mock<ILoggerService>();
                
                user = new User();

                httpContext = new DefaultHttpContext();
                httpContext.Items["user"] = user;

                ControllerContext controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                };
                _logController = new LogController(_loggerServiceMock.Object)
                {
                    ControllerContext = controllerContext
                };
            }

            [TestMethod]
            public void Get_ShouldCallGetLogsByDateOnce()
            {
                //Arrange
                var from = DateTime.Now.AddDays(-1);
                var to = DateTime.Now;
                var loggedUser = new User { Admin = true };
                _logController.HttpContext.Items["user"] = loggedUser;

                //Act
                _logController.Get(from, to);

                //Assert
                _loggerServiceMock.Verify(x => x.GetLogs(from, to, loggedUser), Times.Once);
            }

            [TestMethod]
            public void Get_ShouldReturnOkObjectResult()
            {
                //Arrange
                var from = DateTime.Now.AddDays(-1);
                var to = DateTime.Now;
                var loggedUser = new User { Admin = true };
                var logs = new List<LogEntry> { new LogEntry() };
                _logController.HttpContext.Items["user"] = loggedUser;
                _loggerServiceMock.Setup(x => x.GetLogs(from, to, loggedUser)).Returns(logs);

                //Act
                var result = _logController.Get(from, to);

                //Assert
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            }

            [TestMethod]
            [ExpectedException(typeof(UnauthorizedAccessException))]
            public void Get_ShouldReturnUnauthorizedResult_WhenUserIsNotAdmin()
            {
                //Arrange
                var from = DateTime.Now.AddDays(-1);
                var to = DateTime.Now;
                var loggedUser = new User { Admin = false };
                _logController.HttpContext.Items["user"] = loggedUser;

                var logs = new List<LogEntry> { new LogEntry() };
                _loggerServiceMock.Setup(x => x.GetLogs(from, to, loggedUser)).Throws(new UnauthorizedAccessException());

                //Act
                var result = _logController.Get(from, to);

            }
        }

    }

}
