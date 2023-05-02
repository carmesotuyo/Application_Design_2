using System;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessLogic.Test
{
    [TestClass]
    public class SessionLogicTest
    {
        private Mock<ISessionRepository> sessionRepositoryMock;
        private ISessionLogic sessionLogic;
        private Session session;
        private string username;
        private string password;

        [TestInitialize]
        public void InitTest()
        {
            sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            sessionLogic = new SessionLogic(sessionRepositoryMock.Object);
            session = new Session();
            username = "usernamr";
            password = "password";
        }

        [TestMethod]
        public void LoginOk()
        {
            sessionRepositoryMock!.Setup(x => x.Add(It.IsAny<Session>())).Returns(session);

            var result = sessionLogic!.Login(username, password);
            sessionRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<Guid>(result);
        }
    }
}

