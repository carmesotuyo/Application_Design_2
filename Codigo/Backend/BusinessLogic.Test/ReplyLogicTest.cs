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
            user = new User() { Blogger = true, Id = 1 };
            userAdmin = new User() { Admin = true, Id = 2 };
            reply = new Reply() { User = user };
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
        public void CreateReplytWithoutPermissions()
        {
            Assert.ThrowsException<UnauthorizedAccessException>(() => replyLogic.CreateReply(reply, userAdmin));
        }


        [TestMethod]
        public void DeleteReply()
        {
            replyRepository.Setup(r => r.Get(It.IsAny<Func<Reply, bool>>())).Returns(reply);
            replyRepository.Setup(x => x.Update(It.IsAny<Reply>()));

            replyLogic.DeleteReply(reply.Id, user);

            replyRepository.VerifyAll();
            Assert.IsNotNull(reply.DateDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteReplyWithoutPermissionsTest()
        {
            replyRepository.Setup(r => r.Get(It.IsAny<Func<Reply, bool>>())).Returns(reply);
            replyRepository.Setup(x => x.Update(It.IsAny<Reply>()));

            replyLogic.DeleteReply(reply.Id, userAdmin);
        }
    }
}

