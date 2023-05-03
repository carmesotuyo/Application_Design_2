using BlogsApp.Domain.Entities;
using BlogsApp.Logging.DataAccess.Repositories;
using BlogsApp.Logging.Logic.Services.Services;
using BlogsApp.Logging.Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Logging.Entities;

namespace BlogsApp.Logging.Test
{
    [TestClass]
    public class LoggingServiceTest
    {
        private Mock<ILogEntryRepository> _logEntryRepositoryMock;
        private ILoggerService _loggerService;

        [TestInitialize]
        public void TestInitialize()
        {
            _logEntryRepositoryMock = new Mock<ILogEntryRepository>();
            _loggerService = new DbLoggerService(_logEntryRepositoryMock.Object);
        }

        [TestMethod]
        public void LogLogin_ShouldAddLogEntryToRepository()
        {
            int userId = 1;

            _loggerService.LogLogin(userId);

            _logEntryRepositoryMock.Verify(x => x.Add(It.IsAny<LogEntry>()), Times.Once);
        }

        [TestMethod]
        public void LogSearch_ShouldAddLogEntryToRepository()
        {
            int userId = 1;
            string query = "test";

            _loggerService.LogSearch(userId, query);

            _logEntryRepositoryMock.Verify(x => x.Add(It.IsAny<LogEntry>()), Times.Once);
        }

        [TestMethod]
        public void GetLogsByDate_ShouldThrowUnauthorizedUserException_WhenUserIsNotAdmin()
        {
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;
            User loggedUser = new User
            {
                Admin = false
            };

            Assert.ThrowsException<UnauthorizedUserException>(() => _loggerService.GetLogs(startDate, endDate, loggedUser));
        }

        [TestMethod]
        public void GetLogsByDate_ShouldReturnLogEntriesFromRepository()
        {
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;
            User loggedUser = new User
            {
                Admin = true
            };
            var logEntries = new List<LogEntry>
        {
            new LogEntry { Id = 1, ActionType = "Search", SearchQuery = "test", Timestamp = DateTime.Now },
            new LogEntry { Id = 2, ActionType = "Login", Timestamp = DateTime.Now }
        };
            _logEntryRepositoryMock.Setup(x => x.GetByDate(startDate, endDate)).Returns(logEntries);

            var result = _loggerService.GetLogs(startDate, endDate, loggedUser);

            Assert.AreEqual(logEntries.Count, result.Count);
        }
    }
}
