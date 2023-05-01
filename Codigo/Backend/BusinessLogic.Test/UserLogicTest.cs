using BlogsApp.BusinessLogic.Logics;
using BlogsApp.DataAccess.Interfaces.Exceptions;
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
            aUserRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
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

        [TestMethod]
        public void GetAllUsers()
        {
            aUserRepositoryMock!.Setup(x => x.GetUsers()).Returns(users!);
            userLogic!.GetUsers();
            aUserRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void InsertnullUser()
        {
            userLogic!.InsertUser(nullUser);
            aUserRepositoryMock!.VerifyAll();
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void InsertinvalidUser()
        {
            userLogic!.InsertUser(invalidUser!);
            aUserRepositoryMock!.VerifyAll();
        }

        [TestMethod]
        public void InsertUserOk()
        {
            aUserRepositoryMock!.Setup(x => x.InsertUser(aValidBlogger!));
            userLogic!.InsertUser(aValidBlogger!);
            aUserRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void UpdatenullUser()
        {
            userLogic!.UpdateUser(nullUser);
            aUserRepositoryMock!.VerifyAll();
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void UpdateinvalidUser()
        {
            userLogic!.UpdateUser(invalidUser!);
            aUserRepositoryMock!.VerifyAll();
        }

        [ExpectedException(typeof(ExistenceException))]
        [TestMethod]
        public void UpdateUserNonExist()
        {
            aUserRepositoryMock!.Setup(x => x.GetUserById(aValidBlogger!.Id)).Returns(nullUser);
            userLogic!.UpdateUser(aValidBlogger!);
            aUserRepositoryMock.VerifyAll();
        }
        [TestMethod]
        public void UpdateUserOk()
        {
            aUserRepositoryMock!.Setup(x => x.GetUserById(It.IsAny<int>())).Returns(aValidBlogger!);
            aUserRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()));
            userLogic!.UpdateUser(aValidBlogger!);
            aUserRepositoryMock.VerifyAll();
        }

        [ExpectedException(typeof(ExistenceException))]
        [TestMethod]
        public void DeleteUserNonExist()
        {
            aUserRepositoryMock!.Setup(x => x.GetUserById(10)).Returns(nullUser);
            userLogic!.DeleteUser(10);
            aUserRepositoryMock.VerifyAll();
        }
        [TestMethod]
        public void DeleteUserOk()
        {
            aUserRepositoryMock!.Setup(x => x.GetUserById(1)).Returns(aValidBlogger!);
            aUserRepositoryMock.Setup(x => x.DeleteUser(aValidBlogger!));
            userLogic!.DeleteUser(1);
            aUserRepositoryMock.VerifyAll();
        }
    }
}
