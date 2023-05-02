using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.Controllers;
using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.Domain.Exceptions;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using Microsoft.AspNetCore.Http;

namespace WebApi.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private Mock<IUserLogic>? aUserLogicMock;
        private UserController? aUserControllerMock;
        HttpContext httpContext;
        User aValidBlogger;
        User aBloggerToUpdate;
        int userId;
        CreateUserRequestDTO aValidBloggerDTO;
        UpdateUserRequestDTO updateBloggerRequestDto;

        [TestInitialize]
        public void InitTest()
        {
            aUserLogicMock = new Mock<IUserLogic>(MockBehavior.Default);
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


            userId = 1;
            aBloggerToUpdate = new User
            {
                Id = userId,
                Name = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Blogger = true,
                Admin = false
            };

            updateBloggerRequestDto = new UpdateUserRequestDTO
            {
                Name = "Jane",
                LastName = "Doe",
                Email = "janedoe@example.com",
                Blogger = false,
                Admin = true
            };

            httpContext = new DefaultHttpContext();
            httpContext.Items["user"] = aBloggerToUpdate;

            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            aUserControllerMock = new UserController(aUserLogicMock.Object)
            {
                ControllerContext = controllerContext
            };
        }


        [TestMethod]
        public void PostUserTest()
        {
            aUserLogicMock!.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(aValidBlogger);
            var result = aUserControllerMock!.PostUser(aValidBloggerDTO!);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            aUserLogicMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void PostUserBadRequest()
        {
            var invalidUser = new CreateUserRequestDTO
            {
                Username = aValidBlogger.Username,
                Email = aValidBlogger.Email,
                Name = aValidBlogger.Name,
                LastName = aValidBlogger.LastName,
                Blogger = aValidBlogger.Blogger,
                Admin = aValidBlogger.Admin,
            };

            var result = aUserControllerMock!.PostUser(invalidUser);
            var objectResult = result as BadRequestObjectResult;
            var statusCode = objectResult?.StatusCode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void PatchUserOk()
        {
            var userLogicMock = new Mock<IUserLogic>();
            userLogicMock.Setup(x => x.GetUserById(userId)).Returns(aBloggerToUpdate);
            userLogicMock.Setup(x => x.UpdateUser(aBloggerToUpdate, aBloggerToUpdate)).Verifiable();

            var userController = new UserController(userLogicMock.Object);

            var result = userController.PatchUser(userId, updateBloggerRequestDto);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            userLogicMock.Verify(ul => ul.UpdateUser(aBloggerToUpdate, It.Is<User>(u => u.Id == userId && u.Name == updateBloggerRequestDto.Name && u.LastName == updateBloggerRequestDto.LastName)), Times.Once);

        }


        [TestMethod]
        public void PatchUserFail()
        {
            aUserLogicMock = new Mock<IUserLogic>(MockBehavior.Default);
            aUserLogicMock!.Setup(x => x.UpdateUser(aBloggerToUpdate, aValidBlogger!)).Throws(new Exception());
            var result = aUserControllerMock!.PatchUser(aValidBlogger!.Id, updateBloggerRequestDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            aUserLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }

        

        [TestMethod]
        public void DeleteUserOk()
        {
            aUserLogicMock!.Setup(x => x.DeleteUser(aBloggerToUpdate, aValidBlogger!.Id));
            var result = aUserControllerMock!.DeleteUser(aValidBlogger!.Id);
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            aUserLogicMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }
    }
}

