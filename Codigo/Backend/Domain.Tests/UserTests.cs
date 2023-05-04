using System;
using BlogsApp.Domain.Entities;

namespace Domain.Tests
{
	[TestClass]
	public class UserTests
	{
		[TestMethod]
		public void usernameValid()
		{
			User user = new User() { Username = "valid" };
			Assert.AreEqual(user.Username, "valid");
		}

        [TestMethod]
		[ExpectedException(typeof(InvalidDataException))]
        public void usernameInvalidWithSpaces()
        {
            User user = new User() { Username = "invalid usr" };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void usernameInvalidLong()
        {
            User user = new User() { Username = "invalid_long_username" };
        }

        [TestMethod]
        public void emailValid()
        {
            User user = new User() { Email = "user@valid.com" };
            Assert.AreEqual(user.Email, "user@valid.com");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void emailInvalid()
        {
            User user = new User() { Email = "invalidEmail" };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void emailInvalidNoUser()
        {
            User user = new User() { Email = "@something.com" };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void emailInvalidNoDomain()
        {
            User user = new User() { Email = "user@" };
        }
    }
}

