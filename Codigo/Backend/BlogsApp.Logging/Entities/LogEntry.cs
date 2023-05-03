using BlogsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsApp.Logging.Entities
{
    public class LogEntry
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public string ActionType { get; set; }
        public string SearchQuery { get; set; }
        public DateTime Timestamp { get; set; }

        public LogEntry(int userId, User user, string actionType, string searchQuery, DateTime timestamp)
        {
            if (actionType == null)
            {
                throw new ArgumentException("Action type cannot be null");
            }
            if (searchQuery == null)
            {
                throw new ArgumentException("Search query cannot be null");
            }
            UserId = userId;
            User = user;
            ActionType = actionType;
            SearchQuery = searchQuery;
            Timestamp = timestamp;
        }

        public LogEntry() { }
    }
}
