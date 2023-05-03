using BlogsApp.Logging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogsApp.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.Logging.Repositories
{
    public class LogEntryRepository : ILogEntryRepository
    {
        private readonly Context _context;

        public LogEntryRepository(Context context)
        {
            _context = context;
        }

        public void Add(LogEntry logEntry)
        {
            _context.LogEntries.Add(logEntry);
            _context.SaveChanges();
        }

        public ICollection<LogEntry> GetLogsByActionTypeAndDates(string actionType, DateTime startDate, DateTime endDate)
        {
            return _context.LogEntries
                .Where(log => log.ActionType == actionType && log.Timestamp >= startDate && log.Timestamp <= endDate)
                .Include(log => log.User)
                .ToList();
        }
    }
}
