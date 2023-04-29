using System;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.WebAPI.DTOs;

namespace WebApi.Test
{
	[TestClass]
	public class UserControllerTest
	{
        private Mock<IUserLogic>? mock;
        private UserController? api;
        private User? userA;
        private CreateUserRequestDTO? userADTO;
        private IEnumerable<User>? users;

        [TestInitialize]
        public void InitTest()
        {
            mock = new Mock<IUserLogic>(MockBehavior.Strict);
            api = new UserController(mock.Object);
            userA = new User(
                "carmelauser",
                "password",
                "email@carme.com",
                "carmela",
                "sotuyo",
                true,
                false
            );
            //userADTO = new CreateUserRequestDTO
            //{
            //    Name = userA.Name,
            //    LastName = userA.LastName
            //};
            users = new List<User>() { userA };
        }

        [TestMethod]
        public void GetUsersOk()
		{
            mock!.Setup(x => x.GetUsers()).Returns(users!);

            var result = api!.GetUsers();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            mock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }
	}
}

