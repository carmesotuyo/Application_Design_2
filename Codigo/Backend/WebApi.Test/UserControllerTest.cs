﻿using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.Controllers;
using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.Domain.Exceptions;

namespace WebApi.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private Mock<IUserLogic>? aUserLogicMock;
        private UserController? aUserControllerMock;
        User? aValidBlogger;
        CreateUserRequestDTO aValidBloggerDTO;
        User ExpectedBloggerUser;
        User GivenBloggerUser;
        User DeletedBloggerUser;

        [TestInitialize]
        public void InitTest()
        {
            aUserLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            aUserControllerMock = new UserController(aUserLogicMock.Object);

            aValidBlogger = new User(
                "JPerez",
                "jperez123",
                "jperez@mail.com",
                "Juan",
                "Perez",
                 true,
                 false
            );
            aValidBloggerDTO = new CreateUserRequestDTO
            {
                Username = aValidBlogger.Username,
                Password = aValidBlogger.Password,
                Email = aValidBlogger.Email,
                Name = aValidBlogger.Name,
                LastName = aValidBlogger.LastName,
                Blogger = aValidBlogger.Blogger,
                Admin = aValidBlogger.Admin,
            };
        }


        [TestMethod]
        public void PostUserTest()
        {
            aUserLogicMock!.Setup(x => x.InsertUser(It.IsAny<User>())).Returns(aValidBlogger);
            var result = aUserControllerMock!.PostUser(aValidBloggerDTO!);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            aUserLogicMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void PostUserBadRequest()
        {
            aValidBlogger!.Email = "email";
            aValidBlogger!.Email = "email";
            aUserLogicMock!.Setup(x => x.InsertUser(It.IsAny<User>())).Throws(new BadInputException("El email no es válido"));
            var result = aUserControllerMock!.PostUser(aValidBloggerDTO!);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            aUserLogicMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        //    [TestMethod]
        //    public void PatchUserTest()
        //    {
        //        //var userPatchDTO = new UserPatchDTO { FirstName = "Jane" };

        //        //aUserLogicMock.Setup(x => x.GetUserById(GivenBloggerUser.Id)).Returns(GivenBloggerUser);
        //        //aUserLogicMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Returns(GivenBloggerUser);
        //        //aUserContollerMock = new UserController(aUserLogicMock.Object);
        //        //Assert.IsTrue(GivenBloggerUser.Equals(ExpectedBloggerUser));
        //    }

        //    [TestMethod]
        //    public void DeleteUserTest()
        //    {
        //        //aUserContollerMock = new UserController(aUserLogicMock.Object);
        //        //Assert.IsTrue(GivenBloggerUser.Equals(DeletedBloggerUser));
        //    }

    }
}
