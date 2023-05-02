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
        private Mock<IUserLogic>? userLogicMock;
        private Mock<IArticleLogic> articleLogicMock;
        private UserController? controller;
        HttpContext httpContext;

        User loggedUser;
        User aValidBlogger;
        User aBloggerToUpdate;
        CreateUserRequestDTO aValidBloggerDTO;
        UpdateUserRequestDTO updateBloggerRequestDto;

        [TestInitialize]
        public void InitTest()
        {
            userLogicMock = new Mock<IUserLogic>(MockBehavior.Default);
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Strict);
            controller = new UserController(userLogicMock.Object, articleLogicMock.Object);

            loggedUser = new User() { Id = 1 };
            aValidBlogger = new User(
                "JPerez",
                "jperez123",
                "jperez@mail.com",
                "Juan",
                "Perez",
                 true,
                 false
            );
            aValidBlogger.Id = 2;

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

            aBloggerToUpdate = new User
            {
                Id = 2,
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
            httpContext.Items["user"] = loggedUser;

            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            controller = new UserController(userLogicMock.Object, articleLogicMock.Object)
            {
                ControllerContext = controllerContext
            };
        }


        [TestMethod]
        public void PostUserOkTest()
        {
            userLogicMock!.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(aValidBlogger);

            var result = controller!.PostUser(aValidBloggerDTO);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            userLogicMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void PatchUserOk()
        {
            userLogicMock.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(aBloggerToUpdate);
            userLogicMock.Setup(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<User>())).Returns(aBloggerToUpdate);

            var result = controller.PatchUser(aBloggerToUpdate.Id, updateBloggerRequestDto);

            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            userLogicMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void PatchUserFail()
        {
            userLogicMock.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(aBloggerToUpdate);
            userLogicMock!.Setup(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<User>())).Throws(new UnauthorizedAccessException());

            var result = controller!.PatchUser(aBloggerToUpdate.Id, updateBloggerRequestDto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            userLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void DeleteUserOk()
        {
            userLogicMock!.Setup(x => x.DeleteUser(It.IsAny<User>(), It.IsAny<int>())).Returns(aBloggerToUpdate);

            var result = controller!.DeleteUser(aBloggerToUpdate.Id);
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult?.StatusCode;

            userLogicMock.VerifyAll();
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteUserWithoutPermissions()
        {
            userLogicMock.Setup(m => m.DeleteUser(It.IsAny<User>(), It.IsAny<int>())).Throws(new UnauthorizedAccessException());

            var result = controller!.DeleteUser(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            userLogicMock.VerifyAll();
            Assert.AreEqual(500, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void DeleteUserNotFound()
        {
            userLogicMock.Setup(m => m.DeleteUser(It.IsAny<User>(), It.IsAny<int>())).Throws(new NotFoundDbException());

            var result = controller!.DeleteUser(It.IsAny<int>());
            var objectResult = result as ObjectResult;
            var statusCode = objectResult?.StatusCode;

            articleLogicMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }
    }
}

