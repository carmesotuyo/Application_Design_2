using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogsApp.Logging.Entities;

namespace BlogsApp.Logging.Repositories
{
    public interface ILogEntryRepository
    {
        void Add(LogEntry logEntry);
        ICollection<LogEntry> GetLogsByActionTypeAndDates(string actionType, DateTime startDate, DateTime endDate);
    }
}
