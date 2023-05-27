using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;

namespace BusinessLogic.Test
{
	[TestClass]
	public class CommentLogicTest
    {
        private Mock<ICommentRepository> commentRepository;
        private CommentLogic commentLogic;
        private ICollection<Comment> comments;
        private Comment comment;
        private Comment comment2;
        private User userBlogger;
        private User userAdmin;
        private Article article;

        [TestInitialize]
        public void TestInitialize()
        {
            commentRepository = new Mock<ICommentRepository>(MockBehavior.Strict);
            commentLogic = new CommentLogic(commentRepository.Object);
            userBlogger = new User() { Blogger = true, Id = 1 };
            userAdmin = new User() { Blogger = false, Id = 2 };
            article = new Article() { UserId = userBlogger.Id };
            comment = new Comment() { User = userBlogger, Article = article, DateModified = DateTime.Today };
            comment2 = new Comment() { User = userBlogger, Article = article, DateModified = DateTime.Parse("2022/04/03") };
            comments = new List<Comment>() { comment, comment2 };
        }

        [TestMethod]
        public void CreateComment()
        {
            commentRepository.Setup(x => x.Add(It.IsAny<Comment>())).Returns(comment);

            Comment result = commentLogic.CreateComment(comment, userBlogger);

            commentRepository.VerifyAll();
            Assert.AreEqual(result, comment);
        }

        [TestMethod]
        public void CreateCommentWithoutPermissions()
        {
            Assert.ThrowsException<UnauthorizedAccessException>(() => commentLogic.CreateComment(comment, userAdmin));
        }

        [TestMethod]
        public void DeleteComment()
        {
            commentRepository.Setup(r => r.Get(It.IsAny<Func<Comment, bool>>())).Returns(comment);
            commentRepository.Setup(x => x.Update(It.IsAny<Comment>()));

            commentLogic.DeleteComment(comment.Id, userBlogger);

            commentRepository.VerifyAll();
            Assert.IsNotNull(comment.DateDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteCommentWithoutPermissionsTest()
        {
            commentRepository.Setup(r => r.Get(It.IsAny<Func<Comment, bool>>())).Returns(comment);
            commentRepository.Setup(x => x.Update(It.IsAny<Comment>()));

            commentLogic.DeleteComment(comment.Id, userAdmin);
        }


        [TestMethod]
        public void GetCommentsSinceLasLogout()
        {
            commentRepository.Setup(r => r.GetAll(It.IsAny<Func<Comment, bool>>())).Returns(comments);

            IEnumerable<Comment> result = commentLogic.GetCommentsSince(userBlogger, DateTime.Parse("2023/04/03"));

            commentRepository.VerifyAll();
            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(result.ElementAt(0), comments.ElementAt(0));
        }

        [TestMethod]
        public void CreateSubcomment()
        {
            commentRepository.Setup(x => x.Add(It.IsAny<Comment>())).Returns(comment2);
            commentRepository.Setup(x => x.Update(It.IsAny<Comment>()));

            Comment result = commentLogic.ReplyToComment(comment, comment2, userBlogger);

            commentRepository.VerifyAll();
            Assert.AreEqual(result, comment2);
            Assert.IsTrue(comment.SubComments.Contains(comment2));
        }

        [TestMethod]
        public void CreateSubCommentWithoutPermissions()
        {
            Assert.ThrowsException<UnauthorizedAccessException>(() => commentLogic.ReplyToComment(comment, comment2, userAdmin));
        }
    }
}

