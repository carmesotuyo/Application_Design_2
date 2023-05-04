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
        public void Exists_ShouldReturnTrue_WhenCommentExists()
        {
            // Arrange
            var user = new User("testuser", "testpassword", "testemail@aa.com", "testname", "testlastname", false, false);
            var article = new Article("testarticle", "testbody", 1, user);
            var comment = new Comment(user, "testcomment", article);

            _dbContext.Set<Comment>().Add(comment);
            _dbContext.SaveChanges();

            // Act
            var result = _commentRepository.Exists(c => c.Id == comment.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Exists_ShouldReturnFalse_WhenCommentDoesNotExist()
        {
            // Arrange
            var user = new User("testuser", "testpassword", "testemail@aa.com", "testname", "testlastname", false, false);
            var article = new Article("testarticle", "testbody", 1, user);
            var comment = new Comment(user, "testcomment", article);

            // Act
            var result = _commentRepository.Exists(c => c.Id == comment.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_ShouldReturnComment_WhenCommentExists()
        {
            // Arrange
            _dbContext.Users.Add(_testUser);
            _dbContext.SaveChanges();

            _dbContext.Articles.Add(_testArticle);
            _dbContext.SaveChanges();

            Comment comment = new Comment(_testUser, "testcomment", _testArticle);
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            // Act
            Comment result = _commentRepository.Get(c => c.Id == comment.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(comment.Id, result.Id);
            Assert.AreEqual(comment.Body, result.Body);
            Assert.AreEqual(comment.User.Username, result.User.Username);
            Assert.AreEqual(comment.Article.Name, result.Article.Name);
        }

        [TestMethod]
        public void Get_ShouldThrowNotFoundDbException_WhenCommentDoesNotExist()
        {
            // Arrange
            int nonExistentCommentId = 1;

            // Act & Assert
            Assert.ThrowsException<NotFoundDbException>(() => _commentRepository.Get(c => c.Id == nonExistentCommentId));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllCommentsMatchingCriteria()
        {
            // Arrange
            var comment1 = new Comment(_testUser, "Comment 1", _testArticle);
            var comment2 = new Comment(_testUser, "Comment 2", _testArticle);
            _dbContext.Users.Add(_testUser);
            _dbContext.Users.Add(_testUser2);
            _dbContext.Articles.Add(_testArticle);
            _dbContext.Comments.Add(comment1);
            _dbContext.Comments.Add(comment2);
            _dbContext.SaveChanges();

            // Act
            var comments = _commentRepository.GetAll(c => c.Article.Id == _testArticle.Id);

            // Assert
            Assert.AreEqual(2, comments.Count);
        }

        [TestMethod]
        public void Update_ShouldThrowNotFoundDbException_WhenCommentDoesNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<NotFoundDbException>(() => _commentRepository.Update(_comment));
        }

        [TestMethod]
        public void Update_ShouldUpdateComment_WhenCommentExists()
        {
            _dbContext.Set<Comment>().Add(_comment);
            _dbContext.SaveChanges();

            var updatedComment = _comment;
            updatedComment.DateDeleted = DateTime.UtcNow;

            // Act
            _commentRepository.Update(updatedComment);

            // Assert
            var retrievedComment = _dbContext.Set<Comment>().FirstOrDefault(c => c.Id == _comment.Id);
            Assert.IsNotNull(retrievedComment);
            Assert.IsNotNull(retrievedComment.DateDeleted);
        }
    }
}
