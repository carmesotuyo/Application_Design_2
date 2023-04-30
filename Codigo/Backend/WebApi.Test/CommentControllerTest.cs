using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.Controllers;

namespace WebApi.Test
{
	[TestClass]
	public class CommentControllerTest
    {
        Mock<ICommentLogic> commentLogicMock;
        private CommmentController controller;

        static readonly Comment comment = new Comment() { };

        [TestInitialize]
        public void InitTest()
        {
            commentLogicMock = new Mock<ICommentLogic>(MockBehavior.Strict);
            controller = new CommentController(commentLogicMock.Object);
        }

        [TestMethod]
		public void CreateComment()
		{
            commentLogicMock.Setup(m => m.CreateComment().Returns(comment);

            var result = controller.AddComment(comment);

            commentLogicMock.VerifyAll();
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.IsNotNull(objectResult);

            Assert.IsTrue(objectResult.Equals(comment));
        }
	}
}

