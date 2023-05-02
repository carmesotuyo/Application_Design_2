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

            Session session1 = new Session { User = user, Token = "token1", DateTimeLogin = DateTime.Now };
            _dbContext.Sessions.Add(session1);
            _dbContext.SaveChanges();

            Session session2 = new Session { User = user, Token = "token2", DateTimeLogin = DateTime.Now };

            // Act and assert
            Assert.ThrowsException<AlreadyExistsDbException>(() => sessionRepository.Add(session2));
        }


        [TestMethod]
        public void AddSession_UserDoesNotHaveActiveSession_ReturnsSession()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Password = "testpassword", Email = "papa@123.com", Name = "Pepe", LastName = "Perez" };
            var session = new Session { Id = 1, User = user, Token = "token", DateTimeLogin = DateTime.Now };

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
                Token = "test-token",
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



    }
}
