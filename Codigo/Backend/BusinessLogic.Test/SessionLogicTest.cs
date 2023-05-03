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
        private Mock<ICommentRepository> commentRepositoryMock;
        private ISessionLogic sessionLogic;
        private Mock<ICommentLogic> commentLogicMock;
        private Session session;
        private string username;
        private string password;
        private string incorrectPass;
        private User user;
        private Comment comment;
        private List<Comment> commentsWhileLoggedOut;
        private List<Session> sessions;

        [TestInitialize]
        public void InitTest()
        {
            sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            commentRepositoryMock = new Mock<ICommentRepository>(MockBehavior.Strict);
            commentLogicMock = new Mock<ICommentLogic>(MockBehavior.Strict);
            sessionLogic = new SessionLogic(sessionRepositoryMock.Object, userRepositoryMock.Object, commentLogicMock.Object);
            session = new Session() { Id = 1 };
            username = "usernamr";
            password = "password";
            incorrectPass = "incorrect";
            user = new User() { Username = username, Password = password };
            comment = new Comment();
            commentsWhileLoggedOut = new List<Comment>() { comment };
            sessions = new List<Session>() { session };
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

        [TestMethod]
        public void GetCommentsLoggedOutOk()
        {
            session.DateTimeLogout = DateTime.Parse("2022/03/04");
            sessionRepositoryMock!.Setup(x => x.GetAll(It.IsAny<Func<Session, bool>>())).Returns(sessions);
            commentLogicMock!.Setup(x => x.GetCommentsSince(It.IsAny<DateTime>())).Returns(commentsWhileLoggedOut);

            IEnumerable<Comment> receivedComments = sessionLogic!.GetCommentsWhileLoggedOut(session.Id);
            sessionRepositoryMock.VerifyAll();

            Assert.IsNotNull(receivedComments);
            Assert.AreEqual(receivedComments, commentsWhileLoggedOut);
        }

        [TestMethod]
        public void IsValidTokenOk()
        {
            sessionRepositoryMock!.Setup(x => x.Exists(It.IsAny<Func<Session, bool>>())).Returns(true);
            Assert.IsTrue(sessionLogic.IsValidToken(Guid.NewGuid().ToString()));
        }


        [TestMethod]
        public void InvalidToken()
        {
            sessionRepositoryMock!.Setup(x => x.Exists(It.IsAny<Func<Session, bool>>())).Returns(true);
            Assert.IsFalse(sessionLogic.IsValidToken(null));
        }

        [TestMethod]
        public void GetUserFromToken()
        {
            Guid token = Guid.NewGuid();
            sessionRepositoryMock!.Setup(x => x.Exists(It.IsAny<Func<Session, bool>>())).Returns(true);
            sessionRepositoryMock!.Setup(x => x.Get(It.IsAny<Func<Session, bool>>())).Returns(session);
            User user = sessionLogic.GetUserFromToken(token);

            Assert.AreEqual(user, session.User);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void GetUserFromTokenInvalid()
        {
            Guid token = Guid.NewGuid();
            sessionRepositoryMock!.Setup(x => x.Exists(It.IsAny<Func<Session, bool>>())).Returns(false);
            User user = sessionLogic.GetUserFromToken(token);
        }
    }
}

