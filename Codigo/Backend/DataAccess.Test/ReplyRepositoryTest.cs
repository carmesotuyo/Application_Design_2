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
    public class ReplyRepositoryTest
    {
        private Context _dbContext;
        private ReplyRepository _replyRepository;
        private static User _testUser = new User("username", "password", "email@.com", "name", "last_name", false, false);
        private static User _testUser2 = new User("usernam2", "password", "email@.com", "name", "last_name", false, false);
        private static Article _testArticle = new Article("Test Article", "Test Content", 1, _testUser);
        private Comment _comment = new Comment(_testUser, "Hola", _testArticle);
        private Reply _reply = new Reply(_testUser, "hola");

        [TestInitialize]
        public void TestInit()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "ReplyDb")
                .Options;
            _dbContext = new Context(options);
            _replyRepository = new ReplyRepository(_dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void Add_ShouldAddNewReplyToDatabase()
        {
            var reply = new Reply(_testUser, "Hola");

            // Act
            var result = _replyRepository.Add(reply);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reply.Id, result.Id);
        }

        [TestMethod]
        public void Exists_ShouldReturnTrue_WhenReplyExists()
        {
            var reply = new Reply(_testUser, "Hola");
            _dbContext.Set<Reply>().Add(reply);
            _dbContext.SaveChanges();

            // Act
            var result = _replyRepository.Exists(r => r.Id == reply.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Exists_ShouldReturnFalse_WhenReplyDoesNotExist()
        {
            var reply = new Reply(_testUser, "Hola");
            _dbContext.Set<Reply>().Add(_reply);
            _dbContext.SaveChanges();

            // Act
            var result = _replyRepository.Exists(r => r.Id == _reply.Id + 1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_ShouldReturnReply_WhenReplyExists()
        {
            var reply = new Reply(_testUser, "Body");
            _dbContext.Set<Reply>().Add(reply);
            _dbContext.SaveChanges();

            // Act
            var result = _replyRepository.Get(r => r.Id == reply.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reply.Id, result.Id);
            Assert.AreEqual(reply.User, result.User);
            Assert.AreEqual(reply.Body, result.Body);
        }

        [TestMethod]
        public void Get_ShouldThrowNotFoundDbException_WhenReplyDoesNotExist()
        {
            Assert.ThrowsException<NotFoundDbException>(() => _replyRepository.Get(r => r.Id == 1));
        }
    }
}
