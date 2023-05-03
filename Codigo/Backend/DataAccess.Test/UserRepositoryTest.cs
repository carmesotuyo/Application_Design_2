using BlogsApp.DataAccess;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace DataAccess.Test
{
    [TestClass]
    public class UserRepositoryTest
    {
        private Context _dbContext;
        private UserRepository userRepository;
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
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "UserDb")
                .Options;

            _dbContext = new Context(options);
            userRepository = new UserRepository(_dbContext);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void Add_NewUser_ShouldBeAddedToDatabase()
        {
            var user = new User { Id = 1, Name = "Test User1", Email = "test@example.com", LastName = "Test", Password = "password", Username = "testuser0" };
            
            userRepository.Add(user);
            _dbContext.SaveChanges();

            Assert.AreEqual(1, _dbContext.Users.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsDbException))]
        public void Add_ExistingUser_ShouldThrowAlreadyExistsDbException()
        {
            var existingUser = new User { Username = "testuser", Name = "Test User", Email = "test@example.com", LastName = "Test", Password = "password" };
            _dbContext.Users.Add(existingUser);
            _dbContext.SaveChanges();

            var newUser = new User { Username = "testuser", Name = "Test User 2", Email = "test2@example.com", LastName = "Test 2", Password = "password2" };
            
            userRepository.Add(newUser);
            _dbContext.SaveChanges();
        }
       

        [TestMethod]
        public void Update_UpdatesExistingUser()
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: "UserDb").Options;
            IUserRepository userRepository = new UserRepository(_dbContext);
            _dbContext = new Context(options);
            userRepository = new UserRepository(_dbContext);

            var user = new User
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                LastName = "Doe",
                Password = "password123",
                Username = "john"
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var updatedUser = new User
            {
                Id = 1,
                Name = "Jane Doe",
                Email = "jane@example.com",
                LastName = "Doe",
                Password = "password456",
                Username = "jane"
            };

            userRepository.Update(updatedUser);
            _dbContext.SaveChanges();

            var retrievedUser = _dbContext.Users.Find(user.Id);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual("Jane Doe", retrievedUser.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void Update_NonExistingUser_ShouldThrowNotFoundDbException()
        {
            var user = new User { Id = 1, Name = "Test User" };

            userRepository.Update(user);
        }

        [TestMethod]
        public void Get_ExistingUser_ReturnsUser()
        {
            User user = new User { Id = 1, Name = "Test User", Email = "test@example.com", LastName = "Test", Password = "password", Username = "testuser3" };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            User result = userRepository.Get(u => u.Id == 1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test User", result.Name);
        }

        [TestMethod]
        public void Get_NonExistingUser_ThrowsNotFoundException()
        {
            Assert.ThrowsException<NotFoundDbException>(() => userRepository.Get(u => u.Id == 1));
        }

        [TestMethod]
        public void AddUserFail()
        {
            var context = CreateContext();
            IUserRepository userRepository = CreateUserRepository();

            Assert.ThrowsException<AlreadyExistsDbException>(() => userRepository.Add(aValidBlogger));
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

