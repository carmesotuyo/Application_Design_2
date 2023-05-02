using System;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.Domain.Entities;
using BlogsApp.Domain.Exceptions;
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
        private Mock<IUserRepository> userRepositoryMock;
        private ISessionLogic sessionLogic;
        private Session session;
        private string username;
        private string password;
        private string incorrectPass;
        private User user;

        [TestInitialize]
        public void InitTest()
        {
            sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            sessionLogic = new SessionLogic(sessionRepositoryMock.Object, userRepositoryMock.Object);
            session = new Session() { Id = 1 };
            username = "usernamr";
            password = "password";
            incorrectPass = "incorrect";
            user = new User() { Username = username, Password = password };
        }

        [TestMethod]
        public void LoginOk()
        {
            sessionRepositoryMock!.Setup(x => x.Add(It.IsAny<Session>())).Returns(session);
            userRepositoryMock!.Setup(x => x.Exists(It.IsAny<Func<User, bool>>())).Returns(true);
            userRepositoryMock!.Setup(x => x.Get(It.IsAny<Func<User, bool>>())).Returns(user);

            var result = sessionLogic!.Login(username, password);
            sessionRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<Guid>(result);
        }

        [TestMethod]
        [ExpectedException(typeof(BadInputException))]
        public void LoginIncorrectCreds()
        {
            sessionRepositoryMock!.Setup(x => x.Add(It.IsAny<Session>())).Returns(session);
            userRepositoryMock!.Setup(x => x.Exists(It.IsAny<Func<User, bool>>())).Returns(true);
            userRepositoryMock!.Setup(x => x.Get(It.IsAny<Func<User, bool>>())).Returns(user);

            var result = sessionLogic!.Login(username, incorrectPass);
            sessionRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void LoginUserNotFound()
        {
            sessionRepositoryMock!.Setup(x => x.Add(It.IsAny<Session>())).Returns(session);
            userRepositoryMock!.Setup(x => x.Exists(It.IsAny<Func<User, bool>>())).Returns(false);

            var result = sessionLogic!.Login(username, password);
            sessionRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LogoutOk()
        {
            sessionRepositoryMock!.Setup(x => x.Get(It.IsAny<Func<Session, bool>>())).Returns(session);
            sessionRepositoryMock!.Setup(x => x.Update(It.IsAny<Session>()));

            sessionLogic!.Logout(session.Id, user);
            sessionRepositoryMock.VerifyAll();

            Assert.IsNotNull(session.DateTimeLogout);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void LogoutIncorrectUser()
        {
            sessionRepositoryMock!.Setup(x => x.Get(It.IsAny<Func<Session, bool>>())).Throws(new NotFoundDbException("Session not found"));
            sessionRepositoryMock!.Setup(x => x.Update(It.IsAny<Session>()));

            sessionLogic!.Logout(session.Id, user);
            sessionRepositoryMock.VerifyAll();

            Assert.IsNull(session.DateTimeLogout);
        }
    }
}

