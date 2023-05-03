using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.DataAccess.Repositories;

namespace BusinessLogic.Test
{
	[TestClass]
	public class CommentLogicTest
    {
        private Mock<ICommentRepository> commentRepository;
        private Mock<IReplyLogic> replyLogicMock;
        private CommentLogic commentLogic;
        private ICollection<Comment> comments;
        private Reply reply;
        private Comment comment;
        private User userBlogger;
        private User userAdmin;

        [TestInitialize]
        public void TestInitialize()
        {
            commentRepository = new Mock<ICommentRepository>(MockBehavior.Strict);
            replyLogicMock = new Mock<IReplyLogic>(MockBehavior.Strict);
            commentLogic = new CommentLogic(commentRepository.Object, replyLogicMock.Object);
            reply = new Reply();
            userBlogger = new User() { Blogger = true, Id = 1 };
            userAdmin = new User() { Blogger = false, Id = 2 };
            comment = new Comment() { Reply = reply, User = userBlogger };
            comments = new List<Comment>() { comment };
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
            replyLogicMock.Setup(r => r.DeleteReply(It.IsAny<int>(), It.IsAny<User>()));
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
            replyLogicMock.Setup(r => r.DeleteReply(It.IsAny<int>(), It.IsAny<User>()));
            commentRepository.Setup(x => x.Update(It.IsAny<Comment>()));

            commentLogic.DeleteComment(comment.Id, userAdmin);
        }
    }
}

