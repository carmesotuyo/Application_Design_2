using BlogsApp.DataAccess;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace DataAccess.Test
{
    [TestClass]
    public class UserRepositoryTest
    {
        User aValidBlogger;
        ICollection<User> users;
        Func<User, bool> getById;
        string anotherAddress;
        int Id;
        string name = "newName";

        [TestInitialize]
        public void TestInit()
        {
            aValidBlogger = new User(
               "JPerez",
               "jperez123",
               "jperez@mail.com",
               "Juan",
               "Perez",
                true,
                false
           );
            users = new List<User>() { aValidBlogger };
            getById = GetUserById(aValidBlogger.Id);
            anotherAddress = "5th Avenue";
        }


        [TestMethod]
        public void AddUserOk()
        {
            var context = CreateContext();
            IUserRepository userRepository = new UserRepository(context);

            userRepository.Add(aValidBlogger);
            User userInDb = context.Users.Where<User>(m => m.Id == aValidBlogger.Id).AsNoTracking().FirstOrDefault();

            Assert.IsNotNull(userInDb);
            Assert.AreEqual(aValidBlogger.Id, userInDb.Id);
        }

        [TestMethod]
        public void AddUserFail()
        {
            var context = CreateContext();
            IUserRepository userRepository = CreateUserRepository();

            Assert.ThrowsException<AlreadyExistsDbException>(() => userRepository.Add(aValidBlogger));
        }

        [TestMethod]
        public void GetUserOk()
        {
            var context = CreateContext();
            IUserRepository userRepository = CreateUserRepository();

            User userInDb = userRepository.Get(getById);

            Assert.IsNotNull(userInDb);
            Assert.AreEqual(aValidBlogger.Id, userInDb.Id);
        }


        [TestMethod]
        public void GetUserFail()
        {
            var context = CreateContext();
            IUserRepository userRepository = new UserRepository(context);

            Assert.ThrowsException<NotFoundDbException>(() => userRepository.Get(getById));

        }

        [TestMethod]
        public void GetAllUsersOk()
        {
            var context = CreateContext();
            IUserRepository userRepository = CreateUserRepository();

            ICollection<User> usersInDb = userRepository.GetAll(m => true);

            Assert.IsNotNull(usersInDb);
            Assert.IsTrue(usersInDb.SequenceEqual(users));
        }

        [TestMethod]
        public void GetAllUsersFail()
        {
            var context = CreateContext();
            IUserRepository userRepository = new UserRepository(context);

            Assert.ThrowsException<NotFoundDbException>(() => userRepository.GetAll(m => true));
        }

        [TestMethod]
        public void ExistsUserTrue()
        {
            var context = CreateContext();
            IUserRepository userRepository = CreateUserRepository();

            userRepository.Exists(getById);
            bool existsUser = context.Users.Any<User>(m => m.Id == aValidBlogger.Id);

            Assert.IsTrue(existsUser);
        }

        [TestMethod]
        public void UpdateUserNotFound()
        {
            var context = CreateContext();
            IUserRepository userRepository = new UserRepository(context);
            aValidBlogger.Name = name;

            Assert.ThrowsException<NotFoundDbException>(() => userRepository.Update(aValidBlogger));
        }

        [TestMethod]
        public void UpdateUserOk()
        {
            var context = CreateContext();
            IUserRepository userRepository = CreateUserRepository();
            aValidBlogger.Name = name;

            userRepository.Update(aValidBlogger);
            User updatedUser = context.Users.Where<User>(m => m.Id == aValidBlogger.Id).AsNoTracking().FirstOrDefault();

            Assert.AreEqual(Id, updatedUser.Id);
        }

        private IUserRepository CreateUserRepository()
        {
            var context = CreateContext();

            context.Users?.Add(aValidBlogger!);
            context.SaveChanges();

            IUserRepository userRepository = new UserRepository(context);
            return userRepository;
        }

        private Context CreateContext()
        {
            var contextOptions = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("BlogsAppDb").Options;
            var context = new Context(contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        private Func<User, bool> GetUserById(int id)
        {
            return m => m.Id == id;
        }

    }
}

