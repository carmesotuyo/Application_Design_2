using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccess.Test
{
    [TestClass]
    public class CommentRepositoryTest
    {
        private Context _dbContext;
        private CommentRepository _commentRepository;
        private static User _testUser = new User("username", "password", "email@.com", "name", "last_name", false, false);
        private static User _testUser2 = new User("usernam2", "password", "email@.com", "name", "last_name", false, false);
        private static Article _testArticle = new Article("Test Article", "Test Content", 1, _testUser);
        private Comment _comment = new Comment(_testUser, "Hola", _testArticle);

        [TestInitialize]
        public void TestInit()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "CommentDb")
                .Options;
            _dbContext = new Context(options);
            _commentRepository = new CommentRepository(_dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void Add_ShouldAddCommentToDatabase_WhenCommentDoesNotExist()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                Body = "This is a comment",
                DateCreated = DateTime.Now,
                Article = _testArticle,
                User = _testUser
            };

            // Act
            var result = _commentRepository.Add(comment);

            // Assert
            Assert.AreEqual(comment, result);
            Assert.AreEqual(1, _dbContext.Set<Comment>().Count());
            Assert.IsTrue(_dbContext.Set<Comment>().Any(c => c.Id == comment.Id));
        }

        [TestMethod]
        public void Add_ShouldThrowAlreadyExistsDbException_WhenCommentExists()
        {
            _dbContext.Set<Comment>().Add(_comment);
            _dbContext.SaveChanges();

            // Act and assert
            Assert.ThrowsException<AlreadyExistsDbException>(() => _commentRepository.Add(_comment));
        }
    }
}
