using BlogsApp.Logging.Entities;
using BlogsApp.Logging.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsApp.Logging.Logic.Services
{
    public class DbLoggerService : ILoggerService
    {
        private readonly ILogEntryRepository _logEntryRepository;

        public DbLoggerService(ILogEntryRepository logEntryRepository)
        {
            _logEntryRepository = logEntryRepository;
        }

        public void LogLogin(int userId)
        {
            var logEntry = new LogEntry
            {
                UserId = userId,
                ActionType = "Login",
                Timestamp = DateTime.UtcNow
            };
            _logEntryRepository.Add(logEntry);
        }

        public void LogSearch(int userId, string query)
        {
            var logEntry = new LogEntry
            {
                UserId = userId,
                ActionType = "Search",
                SearchQuery = query,
                Timestamp = DateTime.UtcNow
            };
            _logEntryRepository.Add(logEntry);
        }

        public ICollection<LogEntry> GetLogsByDate(DateTime startDate, DateTime endDate)
        {
            return _logEntryRepository.GetByDate(startDate, endDate);
        }
    }
}
