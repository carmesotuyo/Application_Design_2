using BlogsApp.DataAccess.Repositories;
using BlogsApp.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using System.Runtime.Intrinsics.X86;

namespace DataAccess.Test
{
    [TestClass]
    public class SessionRepositoryTest
    {
        private Context _dbContext;
        private SessionRepository sessionRepository;

        [TestInitialize]
        public void TestInit()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "SessionDb")
                .Options;
            _dbContext = new Context(options);
            sessionRepository = new SessionRepository(_dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void AddSession_UserAlreadyHasActiveSession_ThrowsAlreadyExistsDbException()
        {
            // Arrange
            User user = new User("testuser", "pass", "aa@aa.com", "nano", "nanito", true, false); // Asegúrate de que el email termine en ".com"
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            Session session1 = new Session { User = user, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now };
            _dbContext.Sessions.Add(session1);
            _dbContext.SaveChanges();

            Session session2 = new Session { User = user, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now };

            // Act and assert
            Assert.ThrowsException<AlreadyExistsDbException>(() => sessionRepository.Add(session2));
        }


        [TestMethod]
        public void AddSession_UserDoesNotHaveActiveSession_ReturnsSession()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Password = "testpassword", Email = "papa@123.com", Name = "Pepe", LastName = "Perez" };
            var session = new Session { Id = 1, User = user, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now };

            // Act
            var result = sessionRepository.Add(session);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(session.Id, result.Id);
            Assert.AreEqual(session.User, result.User);
            Assert.AreEqual(session.Token, result.Token);
            Assert.AreEqual(session.DateTimeLogin, result.DateTimeLogin);
            Assert.IsNull(result.DateTimeLogout);
        }

        [TestMethod]
        public void Add_CreatesNewSession_WhenNoActiveSessionExists()
        {
            // Arrange
            var user2 = new User
            {
                Id = 1,
                // Agrega aquí las propiedades requeridas adicionales
            };
            User user = new User("testuser", "pass", "aa@aa.com", "nano", "nanito", true, false);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var newSession = new Session
            {
                User = user,
                Token = Guid.NewGuid(),
                DateTimeLogin = DateTime.UtcNow
            };

            // Act
            var createdSession = sessionRepository.Add(newSession);

            // Assert
            Assert.IsNotNull(createdSession);
            Assert.AreEqual(newSession.Token, createdSession.Token);
            Assert.AreEqual(newSession.DateTimeLogin, createdSession.DateTimeLogin);
            Assert.IsNull(createdSession.DateTimeLogout);
        }

        [TestMethod]
        public void Update_SessionExists_ShouldUpdateSession()
        {
            // Arrange
            var session = new Session
            {
                Id = 1,
                User = new User("testuser", "pass", "aa@aa.com", "nano", "nanito", true, false),
                Token = Guid.NewGuid(),
                DateTimeLogin = DateTime.Now,
                DateTimeLogout = null
            };
            _dbContext.Sessions.Add(session);
            _dbContext.SaveChanges();

            var updatedSession = new Session
            {
                Id = session.Id,
                User = session.User,
                Token = Guid.NewGuid(),
                DateTimeLogin = session.DateTimeLogin,
                DateTimeLogout = DateTime.Now
            };

            // Act
            sessionRepository.Update(updatedSession);

            // Assert
            var result = _dbContext.Sessions.Find(session.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedSession.Token, result.Token);
            Assert.AreEqual(updatedSession.DateTimeLogout, result.DateTimeLogout);
        }

        [TestMethod]
        public void Update_SessionNotExists_ShouldThrowNotFoundDbException()
        {
            // Arrange
            var session = new Session
            {
                Id = 1,
                User = new User("testuser", "pass", "aa@aa.com", "nano", "nanito", true, false),
                Token = Guid.NewGuid(),
                DateTimeLogin = DateTime.Now,
                DateTimeLogout = null
            };

            // Act & Assert
            Assert.ThrowsException<NotFoundDbException>(() => sessionRepository.Update(session));
        }

        [TestMethod]
        public void Get_SessionExists_ReturnsSession()
        {
            // Arrange
            var user = new User("testuser", "pass", "aa@aa.com", "nano", "nanito", true, false);
            var session = new Session { Id = 1, Token = Guid.NewGuid(), User = user, DateTimeLogin = DateTime.Now };
            _dbContext.Set<Session>().Add(session);
            _dbContext.SaveChanges();

            // Act
            var result = sessionRepository.Get(s => s.Id == session.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(session.Id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void Get_SessionDoesNotExist_ThrowsNotFoundDbException()
        {
            // Arrange
            var user = new User("testuser", "pass", "aa@aa.com", "nano", "nanito", true, false);
            var session = new Session { Id = 1, Token = Guid.NewGuid(), User = user, DateTimeLogin = DateTime.Now };
            _dbContext.Set<Session>().Add(session);
            _dbContext.SaveChanges();

            // Act
            sessionRepository.Get(s => s.Id == 2);
        }

        [TestMethod]
        public void GetSessions_Successful()
        {
            // Arrange
            var user1 = new User("testuser", "pass", "aa@aa.com", "nano", "nanito", true, false);
            var user2 = new User("testuser2", "pass", "ba@aa.com", "nano", "nanito", true, false);
            var session1 = new Session { Id = 1, User = user1, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now.AddMinutes(-30) };
            var session2 = new Session { Id = 2, User = user2, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now.AddDays(-1), DateTimeLogout = DateTime.Now };
            var session3 = new Session { Id = 3, User = user1, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now.AddHours(-2), DateTimeLogout = DateTime.Now.AddHours(-1) };
            _dbContext.Users.AddRange(new List<User> { user1, user2 });
            _dbContext.Sessions.AddRange(new List<Session> { session1, session2, session3 });
            _dbContext.SaveChanges();

            // Act
            var result = sessionRepository.GetAll(s => s.User == user1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(s => s.Id == 1));
            Assert.IsTrue(result.Any(s => s.Id == 3));
        }

        [TestMethod]
        public void GetSessions_NoSessionsFound()
        {
            // Arrange
            var user1 = new User("testuser2", "pass", "ba@aa.com", "nano", "nanito", true, false);
            _dbContext.Users.Add(user1);
            _dbContext.SaveChanges();

            // Act
            var result = sessionRepository.GetAll(s => s.User == user1);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Exists_SessionExists_ReturnsTrue()
        {
            // Arrange
            var user1 = new User("testuser2", "pass", "ba@aa.com", "nano", "nanito", true, false);
            var session = new Session { Id = 1, User = user1, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now.AddMinutes(-30) };
            _dbContext.Set<Session>().Add(session);
            _dbContext.SaveChanges();

            // Act
            var result = sessionRepository.Exists(s => s.Id == session.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Exists_SessionDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var user1 = new User("testuser2", "pass", "ba@aa.com", "nano", "nanito", true, false);
            var session = new Session { Id = 1, User = user1, Token = Guid.NewGuid(), DateTimeLogin = DateTime.Now.AddMinutes(-30) };
            _dbContext.Set<Session>().Add(session);
            _dbContext.SaveChanges();

            // Act
            var result = sessionRepository.Exists(s => s.Id == 2);

            // Assert
            Assert.IsFalse(result);
        }

    }
}
