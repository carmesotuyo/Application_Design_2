using BlogsApp.Domain.Entities;
using BlogsApp.Logging.DataAccess.Repositories;
using BlogsApp.Logging.Logic.Services.Services;
using BlogsApp.Logging.Logic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Logging.Entities;
using BlogsApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace BlogsApp.Logging.Test
{
    [TestClass]
    public class LoggingRepositoryTest
    {
        private Context _dbContext;
        private LogEntryRepository _logEntryRepository;

        [TestInitialize]
        public void TestInit()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "LogEntryDb")
                .Options;
            _dbContext = new Context(options);
            _logEntryRepository = new LogEntryRepository(_dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void Add_ShouldIncreaseCountOfLogEntries_WhenLogEntryIsAdded()
        {
            var logEntry = new LogEntry(1, "Test", "Test Query", DateTime.UtcNow);

            _logEntryRepository.Add(logEntry);

            Assert.AreEqual(1, _dbContext.LogEntries.Count());
        }

        [TestMethod]
        public void GetByDate_ShouldReturnCorrectLogEntries_WhenDateRangeIsGiven()
        {

            LogEntry log1 = new LogEntry(1, "Test", "Test Query 1", DateTime.UtcNow.AddDays(-2));
            LogEntry log2 = new LogEntry(2, "Test", "Test Query 2", DateTime.UtcNow.AddDays(-2));
            LogEntry log3 = new LogEntry(1, "Test", "Test Query 3", DateTime.UtcNow);
            _dbContext.LogEntries.Add(log1);
            _dbContext.SaveChanges();
            _dbContext.LogEntries.Add(log2);
            _dbContext.SaveChanges();
            _dbContext.LogEntries.Add(log3);
            _dbContext.SaveChanges();

            var startDate = DateTime.UtcNow.AddDays(-3);
            var endDate = DateTime.UtcNow.AddDays(-1);  //DateTime.UtcNow;
            var result = _logEntryRepository.GetByDate(startDate, endDate);

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(l => l.Timestamp >= startDate && l.Timestamp <= endDate));
        }

    }
}
