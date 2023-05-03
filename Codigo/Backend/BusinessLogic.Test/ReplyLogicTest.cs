using System;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Domain.Entities;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.IBusinessLogic.Interfaces;

namespace BusinessLogic.Test
{
	[TestClass]
	public class ReplyLogicTest
	{
        private Mock<IReplyRepository> replyRepository;
		private ReplyLogic replyLogic;
        private Reply reply;
        private User user;
        private User userAdmin;

        [TestInitialize]
        public void TestInitialize()
        {
            replyRepository = new Mock<IReplyRepository>(MockBehavior.Strict);
            replyLogic = new ReplyLogic(replyRepository.Object);
            reply = new Reply();
            user = new User() { Blogger = true };
            userAdmin = new User() { Admin = true };
        }

        [TestMethod]
        public void CreateReply()
        {
            replyRepository.Setup(x => x.Add(It.IsAny<Reply>())).Returns(reply);

            Reply result = replyLogic.CreateReply(reply, user);

            replyRepository.VerifyAll();
            Assert.AreEqual(result, reply);
        }

        [TestMethod]
        public void CreateCommentWithoutPermissions()
        {
            Assert.ThrowsException<UnauthorizedAccessException>(() => replyLogic.CreateReply(reply, userAdmin));
        }
    }
}

