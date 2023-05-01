using System;
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
        private readonly Comment comment = new Comment();
        private readonly User userBlogger = new User() { Blogger = true };

        [TestInitialize]
        public void TestInitialize()
        {
            commentRepository = new Mock<ICommentRepository>(MockBehavior.Strict);
            commentLogic = new CommentLogic(commentRepository.Object);
            comments = new List<Comment>() { comment };
        }

        [TestMethod]
        public void CreateComment()
        {
            commentRepository.Setup(x => x.Add(It.IsAny<Comment>())).Returns(comment);
            commentRepository.Setup(x => x.Exists(It.IsAny<Func<Comment, bool>>())).Returns(false);

            Comment result = commentLogic.CreateComment(comment, userBlogger);

            commentRepository.VerifyAll();
            Assert.AreEqual(result, comment);
        }
    }
}

