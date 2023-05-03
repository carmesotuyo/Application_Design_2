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
using NuGet.Common;
using BlogsApp.Logging.Logic.Services.Services;
using BlogsApp.Logging.Logic.Services.Services;

namespace WebApi.Test
{
    [TestClass]
    public class LogControllerTests
    {
        private LogController _logController;
        private Mock<ILoggerService> _loggerServiceMock;
        private Mock<ISessionLogic> _sessionLogicMock;
        HttpContext httpContext;
        private User user;
        private Guid token;

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerServiceMock = new Mock<ILoggerService>();
            _sessionLogicMock = new Mock<ISessionLogic>();
            _logController = new LogController(_loggerServiceMock.Object, _sessionLogicMock.Object);
            user = new User();
            token = Guid.NewGuid();
        }

        [TestMethod]
        public void Get_ShouldCallGetLogsByDateOnce()
        {
            //Arrange
            var from = DateTime.Now.AddDays(-1);
            var to = DateTime.Now;
            var loggedUser = new User { Admin = true };
            _sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(loggedUser);

            //Act
            _logController.Get(from, to, token.ToString());

            //Assert
            _loggerServiceMock.Verify(x => x.GetLogs(from, to, loggedUser), Times.Once);
            _sessionLogicMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnOkObjectResult()
        {
            //Arrange
            var from = DateTime.Now.AddDays(-1);
            var to = DateTime.Now;
            var loggedUser = new User { Admin = true };
            var logs = new List<LogEntry> { new LogEntry() };
            _loggerServiceMock.Setup(x => x.GetLogs(from, to, loggedUser)).Returns(logs);
            _sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(user);

            //Act
            var result = _logController.Get(from, to, token.ToString());

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _sessionLogicMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedUserException))]
        public void Get_ShouldReturnUnauthorizedResult_WhenUserIsNotAdmin()
        {
            //Arrange
            var from = DateTime.Now.AddDays(-1);
            var to = DateTime.Now;
            var loggedUser = new User { Admin = false };
            _sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(loggedUser);

            var logs = new List<LogEntry> { new LogEntry() };
            _loggerServiceMock.Setup(x => x.GetLogs(from, to, loggedUser)).Throws(new UnauthorizedUserException(""));

            //Act
            var result = _logController.Get(from, to, token.ToString());
            _sessionLogicMock.VerifyAll();

        }
    }
}
