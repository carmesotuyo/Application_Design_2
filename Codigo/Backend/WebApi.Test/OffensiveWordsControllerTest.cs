using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.Domain.Entities;
using System.Data;
using BlogsApp.WebAPI.Controllers;
using System.Net.Http;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.Logging.Logic.Services;
using BlogsApp.WebAPI.DTOs;

namespace WebApi.Test
{
	[TestClass]
	public class OffensiveWordsControllerTest
    {
        Mock<IOffensiveWordsValidator> offensiveWordValidatorMock;
        private Mock<ISessionLogic> sessionLogicMock;
        private OffensiveWordsController controller;

        private OffensiveWordDTO wordDto;
        private User moderator;
        private User commonUser;
        private Guid token;

        [TestInitialize]
        public void InitTest()
        {
            offensiveWordValidatorMock = new Mock<IOffensiveWordsValidator>(MockBehavior.Strict);
            sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            controller = new OffensiveWordsController(offensiveWordValidatorMock.Object, sessionLogicMock.Object);

            wordDto = new OffensiveWordDTO() { word = "offensive" };
            moderator = new User() { Moderador = true };
            commonUser = new User() { Moderador = false, Admin = false };
            token = Guid.NewGuid();
        }

        [TestMethod]
        public void AddOffensiveWord()
        {
            offensiveWordValidatorMock.Setup(m => m.AddOffensiveWord(It.IsAny<User>(), It.IsAny<string>()));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(moderator);

            var result = controller.AddOffensiveWord(wordDto, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            offensiveWordValidatorMock.VerifyAll();
            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AddOffensiveWordWithoutPermissions()
        {
            offensiveWordValidatorMock.Setup(m => m.AddOffensiveWord(It.IsAny<User>(), It.IsAny<string>())).Throws(new UnauthorizedAccessException());
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(commonUser);

            var result = controller.AddOffensiveWord(wordDto, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            offensiveWordValidatorMock.VerifyAll();
            sessionLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }

        [TestMethod]
        public void RemoveOffensiveWord()
        {
            offensiveWordValidatorMock.Setup(m => m.RemoveOffensiveWord(It.IsAny<User>(), It.IsAny<string>()));
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(moderator);

            var result = controller.RemoveOffensiveWord(wordDto, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            offensiveWordValidatorMock.VerifyAll();
            sessionLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void RemoveOffensiveWordWithoutPermissions()
        {
            offensiveWordValidatorMock.Setup(m => m.RemoveOffensiveWord(It.IsAny<User>(), It.IsAny<string>())).Throws(new UnauthorizedAccessException());
            sessionLogicMock!.Setup(m => m.GetUserFromToken(It.IsAny<Guid>())).Returns(commonUser);

            var result = controller.RemoveOffensiveWord(wordDto, token.ToString());
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            offensiveWordValidatorMock.VerifyAll();
            sessionLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }
    }
}

