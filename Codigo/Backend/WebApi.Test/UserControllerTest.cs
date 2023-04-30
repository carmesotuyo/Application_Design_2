using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.Controllers;
using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace WebApi.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private Mock<IUserLogic> aUserLogicMock;
        private UserController? aUserContollerMock;
        User ExpectedBloggerUser;
        User GivenBloggerUser;
        User DeletedBloggerUser;

        [TestInitialize]
        public void InitTest()
        {
            aUserLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);

            User ExpectedBloggerUser = new User()
            {
                Username = "Mocked User",
                Name = "Mocked Name",
                LastName = "Mocked Last Name",
                Email = "mockedEmail@gmail.com",
                Password = "mockerpass123",
                Blogger = true,
                Admin = false
            };

             User GivenBloggerUser = new User()
            {
                Username = "Mocked User",
                Name = "Mocked Name",
                LastName = "Mocked Last Name",
                Email = "mockedEmail@gmail.com",
                Password = "mockerpass123",
                Blogger = true,
                Admin = false
            };

            User DeletedBloggerUser = new User()
            {
                Username = "Mocked User",
                Name = "Mocked Name",
                LastName = "Mocked Last Name",
                Email = "mockedEmail@gmail.com",
                Password = "mockerpass123",
                Blogger = true,
                Admin = false,
                DateDeleted = DateTime.Now
            };
    }


        [TestMethod]
        public void PostUserTest()
        {
            aUserContollerMock = new UserController(aUserLogicMock.Object);
            aUserLogicMock.Setup(m => m.CreateUser(It.IsAny<User>())).Returns(ExpectedBloggerUser);

            var resultCall = aUserContollerMock.AddUser(GivenBloggerUser);

            Assert.IsTrue(GivenBloggerUser.Equals(ExpectedBloggerUser));
        }

        [TestMethod]
        public void PatchUserTest()
        {
            aUserContollerMock = new UserController(aUserLogicMock.Object);
            Assert.IsTrue(GivenBloggerUser.Equals(ExpectedBloggerUser));
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            aUserContollerMock = new UserController(aUserLogicMock.Object);
            Assert.IsTrue(GivenBloggerUser.Equals(DeletedBloggerUser));
        }

    }
}
