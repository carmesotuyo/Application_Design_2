using BlogsApp.Domain.Entities;
using BlogsApp.Logging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsApp.Logging.Logic.Services
{
    public interface ILoggerService
    {
        void LogLogin(int userId);
        public void LogSearch(int userId, string query);
        ICollection<LogEntry> GetLogs(DateTime startDate, DateTime endDate, User loggedUser);
    }
}
