using BlogsApp.BusinessLogic.Logics;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.Domain.Entities;
using BlogsApp.Domain.Exceptions;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessLogic.Test
{
    [TestClass]
    public class UserLogicTest
    {
        private Mock<IUserRepository>? aUserRepositoryMock;
        private UserLogic? userLogic;
        private User? aValidBlogger;
        private User? nullUser;
        private User? invalidUser;
        private IEnumerable<User>? users;

        [TestInitialize]
        public void InitTest()
        {
            aUserRepositoryMock = new Mock<IUserRepository>(MockBehavior.Default);
            userLogic = new UserLogic(aUserRepositoryMock.Object);
            aValidBlogger = new User(
                "JPerez",
                "jperez123",
                "jperez@mail.com",
                "Juan",
                "Perez",
                 true,
                 false
            );
            nullUser = null;
            users = new List<User>() { aValidBlogger };
            invalidUser = new User("", "", "", "", "", false, false);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void CreatenullUser()
        {
            userLogic!.CreateUser(nullUser);
            aUserRepositoryMock!.VerifyAll();
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void InsertinvalidUser()
        {
            userLogic!.CreateUser(invalidUser!);
            aUserRepositoryMock!.VerifyAll();
        }

        [TestMethod]
        public void InsertUserOk()
        {
            aUserRepositoryMock!.Setup(x => x.Add(aValidBlogger!));
            userLogic!.CreateUser(aValidBlogger!);
            aUserRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void SetsDateDeletedTest()
        {
            aUserRepositoryMock.Setup(r => r.Get(It.IsAny<Func<User, bool>>())).Returns(aValidBlogger);

            userLogic.DeleteUser(aValidBlogger.Id);
            User result = aValidBlogger;

            aUserRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);

            Assert.IsNotNull(aValidBlogger.DateDeleted);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            aUserRepositoryMock.Setup(x => x.Get(It.IsAny<Func<User, bool>>())).Returns(aValidBlogger);
            User result = userLogic.GetUserById(aValidBlogger.Id);
            aUserRepositoryMock.VerifyAll();
            Assert.IsTrue(result.Id == aValidBlogger.Id);
        }

        [TestMethod]
        public void UpdateUserNameTest()
        {
            int userId = 1;
            User existingUser = new User { Id = userId, Name = "Old name" };
            User updatedUser = new User { Name = "New name" };
            aUserRepositoryMock.Setup(repo => repo.Get(It.IsAny<Func<User, bool>>())).Returns(existingUser);

            User result = userLogic.UpdateUser(userId, updatedUser);

            aUserRepositoryMock.Verify(repo => repo.Update(existingUser), Times.Once);
            Assert.AreEqual(updatedUser.Name, result.Name);

        }

        [TestMethod]
        public void UpdateUserPassTest()
        {
            int userId = 1;
            User existingUser = new User { Id = userId, Password = "OldPassword" };
            User updatedUser = new User { Password = "NewPassword" };
            aUserRepositoryMock.Setup(repo => repo.Get(It.IsAny<Func<User, bool>>())).Returns(existingUser);

            User result = userLogic.UpdateUser(userId, updatedUser);

            aUserRepositoryMock.Verify(repo => repo.Update(existingUser), Times.Once);
            Assert.AreEqual(updatedUser.Password, result.Password);
        }
    }
}
