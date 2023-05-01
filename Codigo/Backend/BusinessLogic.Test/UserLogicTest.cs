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
        private Mock<IUserRepository> aUserRepositoryMock;
        private IUserLogic userLogic;       
        User? aValidBlogger;
        private IEnumerable<User>? users = new List<User>();
        User invalidUser; 
        private static readonly User user = new User { Id = 1, Name = "Test User", LastName = "Last Test User" };


        [TestInitialize]
        public void TestInitialize()
        {
            aUserRepositoryMock = new Mock<IUserRepository>(MockBehavior.Default);
            userLogic = new UserLogic(aUserRepositoryMock.Object);
           
        }

        
        [TestMethod]
        [ExpectedException(typeof(BadInputException))]
        public void CreatenullUser()
        {
            userLogic!.CreateUser(null);
            aUserRepositoryMock!.VerifyAll();
        }


        [TestMethod]
        public void InsertUserOk()
        {
            UserLogic userLogic = new UserLogic(aUserRepositoryMock.Object);
            aUserRepositoryMock!.Setup(x => x.Add(It.IsAny<User>())).Returns(user);

            var result = userLogic!.CreateUser(user!);
            aUserRepositoryMock.VerifyAll();

            Assert.IsNotNull(result); 
            Assert.AreEqual(user.Name, result.Name);
            Assert.AreEqual(user.LastName, result.LastName); 
                                               
        }

        [TestMethod]
        public void SetsDateDeletedTest()
        {
            aUserRepositoryMock.Setup(r => r.Get(It.IsAny<Func<User, bool>>())).Returns(user);

            userLogic.DeleteUser(user.Id);
            User result = user;

            aUserRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);

            Assert.IsNotNull(user.DateDeleted);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            aUserRepositoryMock.Setup(x => x.Get(It.IsAny<Func<User, bool>>())).Returns(user);
            User result = userLogic.GetUserById(user.Id);
            aUserRepositoryMock.VerifyAll();
            Assert.IsTrue(result.Id == user.Id);
        }

        [TestMethod]
        public void UpdateUserNameTest()
        {
            int userId = 2;
            User existingUser = new User { Id = userId, Name = "Old name" };
            User updatedUser = new User { Name = "New name" };
   
            aUserRepositoryMock.Setup(x => x.Get(It.IsAny<Func<User, bool>>())).Returns(existingUser);

            User result = userLogic.UpdateUser(updatedUser);

            aUserRepositoryMock.Verify(x => x.Update(existingUser), Times.Once);
            Assert.AreEqual(updatedUser.Name, result.Name);
        }

        [TestMethod]
        public void UpdateUserPassTest()
        {
            int userId = 1;
            User existingUser = new User { Id = userId, Password = "OldPassword" };
            User updatedUser = new User { Password = "NewPassword" };
            aUserRepositoryMock.Setup(repo => repo.Get(It.IsAny<Func<User, bool>>())).Returns(existingUser);

            User result = userLogic.UpdateUser(updatedUser);

            aUserRepositoryMock.Verify(repo => repo.Update(existingUser), Times.Once);
            Assert.AreEqual(updatedUser.Password, result.Password);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            aUserRepositoryMock.Setup(r => r.Get(It.IsAny<Func<User, bool>>())).Returns(user);

            userLogic.DeleteUser(user.Id);
            User result = user;

            aUserRepositoryMock.VerifyAll();
            Assert.IsTrue(user.DateDeleted != null);

            Assert.IsNotNull(user.DateDeleted);
        }
    }
}
