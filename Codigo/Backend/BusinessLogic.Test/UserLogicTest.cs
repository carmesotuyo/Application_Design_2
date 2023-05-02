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
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IArticleLogic> articleLogicMock;
        private IUserLogic userLogic;       
        User? aValidBlogger;
        private IEnumerable<User>? users = new List<User>();
        User invalidUser;
        private User adminUser;
        private User normalUser;
        private User normalUser2;
        private Article article1;
        private Article article2;
        private static readonly User user = new User { Id = 1, Name = "Test User", LastName = "Last Test User" };


        [TestInitialize]
        public void TestInitialize()
        {
            userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Default);
            articleLogicMock = new Mock<IArticleLogic>(MockBehavior.Default);
            userLogic = new UserLogic(userRepositoryMock.Object, articleLogicMock.Object);

            adminUser = new User { Id = 1, Username = "admin", Admin = true };
            normalUser = new User { Id = 2, Username = "user", Blogger = true };
            normalUser2 = new User { Id = 3, Username = "blogger", Blogger = true };
            article1 = new Article { Id = 1, UserId = 2 };
            article2 = new Article { Id = 2, UserId = 2 };
            normalUser.Articles = new List<Article> { article1, article2 };
        }

        
        [TestMethod]
        [ExpectedException(typeof(BadInputException))]
        public void CreateNullUser()
        {
            userLogic!.CreateUser(null);
            userRepositoryMock!.VerifyAll();
        }


        [TestMethod]
        public void CreateUserOk()
        {
            userRepositoryMock!.Setup(x => x.Add(It.IsAny<User>())).Returns(user);

            var result = userLogic!.CreateUser(user!);
            userRepositoryMock.VerifyAll();

            Assert.IsNotNull(result); 
            Assert.AreEqual(user.Name, result.Name);
            Assert.AreEqual(user.LastName, result.LastName);                   
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            userRepositoryMock.Setup(x => x.Get(It.IsAny<Func<User, bool>>())).Returns(user);
            User result = userLogic.GetUserById(user.Id);
            userRepositoryMock.VerifyAll();
            Assert.IsTrue(result.Id == user.Id);
        }

        [TestMethod]
        public void UpdateName()
        {
            userRepositoryMock.Setup(r => r.Exists(It.IsAny<Func<User, bool>>())).Returns(true);
            userRepositoryMock.Setup(r => r.Get(It.IsAny<Func<User, bool>>())).Returns(normalUser);

            normalUser.Name = "Jonathan";
            User updatedUser = userLogic.UpdateUser(normalUser, normalUser);

            userRepositoryMock.VerifyAll();
            Assert.AreEqual("Jonathan", updatedUser.Name);
        }


        [TestMethod]
        public void UpdateUserName()
        {
            userRepositoryMock.Setup(r => r.Exists(It.IsAny<Func<User, bool>>())).Returns(true);
            userRepositoryMock.Setup(r => r.Get(It.IsAny<Func<User, bool>>())).Returns(normalUser);

            normalUser.Username = "Jonathan";
            User updatedUser = userLogic.UpdateUser(normalUser, normalUser);

            userRepositoryMock.VerifyAll();
            Assert.AreEqual("Jonathan", updatedUser.Username);
        }


        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void UpdateUserNonExistingUser()
        {
            userRepositoryMock.Setup(r => r.Exists(It.IsAny<Func<User, bool>>())).Returns(false);

            userLogic.UpdateUser(normalUser, user);
        }


        [TestMethod]
        public void DeleteUserAndArticlesTest()
        {
            userRepositoryMock.Setup(r => r.Exists(It.IsAny<Func<User, bool>>())).Returns(true);
            userRepositoryMock.Setup(r => r.Get(It.IsAny<Func<User, bool>>())).Returns(normalUser);

            var deletedUser = userLogic.DeleteUser(adminUser, normalUser.Id);

            userRepositoryMock.Verify(r => r.Update(normalUser), Times.Once);
            articleLogicMock.Verify(a => a.DeleteArticle(article1.Id, normalUser), Times.Once);
            articleLogicMock.Verify(a => a.DeleteArticle(article2.Id, normalUser), Times.Once);
            Assert.IsNotNull(deletedUser.DateDeleted);
        }

        [TestMethod]
        public void DeleteUserInvalidUser()
        {
            userRepositoryMock.Setup(r => r.Exists(It.IsAny<Func<User, bool>>())).Returns(false);

            Assert.ThrowsException<NotFoundDbException>(() => userLogic.DeleteUser(adminUser, normalUser.Id));
        }

        [TestMethod]
        public void DeleteUserUnauthorizedUser()
        {
            Assert.ThrowsException<UnauthorizedAccessException>(() => userLogic.DeleteUser(normalUser, adminUser.Id));
        }

        [TestMethod]
        public void GetUsersRanking()
        {
            var dateFrom = new DateTime(2023, 1, 1);
            var dateTo = new DateTime(2023, 5, 1);

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Articles = new List<Article>
                    {
                        new Article { Id = 1, DateCreated = new DateTime(2023, 1, 15) },
                        new Article { Id = 2, DateCreated = new DateTime(2023, 2, 20) },
                    },
                    Comments = new List<Comment>
                    {
                        new Comment { Id = 1, DateCreated = new DateTime(2023, 1, 10) },
                    }
                },
                new User
                {
                    Id = 2,
                    Articles = new List<Article>
                    {
                        new Article { Id = 3, DateCreated = new DateTime(2023, 3, 10) },
                    },
                    Comments = new List<Comment>
                    {
                        new Comment { Id = 2, DateCreated = new DateTime(2023, 3, 20) },
                        new Comment { Id = 3, DateCreated = new DateTime(2023, 4, 15) },
                    }
                },
                new User
                {
                    Id = 3,
                    Articles = new List<Article>(),
                    Comments = new List<Comment>()
                }
            };

            userRepositoryMock.Setup(x => x.GetAll(It.IsAny<Func<User, bool>>()))
                   .Returns<Func<User, bool>>(filter => users.Where(filter).ToList());

            var result = userLogic.GetUsersRanking(adminUser, dateFrom, dateTo, null);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result.First().Id);
            Assert.AreEqual(2, result.Last().Id);
        }

        [TestMethod]
        public void GetUsersRankingUnauthorizedUser()
        {
            var dateFrom = new DateTime(2023, 1, 1);
            var dateTo = new DateTime(2023, 5, 1);

            Assert.ThrowsException<UnauthorizedAccessException>(() => userLogic.GetUsersRanking(normalUser, dateFrom, dateTo, null));
        }

    }
}
