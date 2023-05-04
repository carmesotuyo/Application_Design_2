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
        public int UserId { get; set; }
        public string ActionType { get; set; }
        public string? SearchQuery { get; set; }
        public DateTime Timestamp { get; set; }

        public LogEntry(int userId, string actionType, string searchQuery, DateTime timestamp)
        {
            if (userId == null)
            {
                throw new ArgumentException("UserId no debe ser null");
            }
            if (actionType == null)
            {
                throw new ArgumentException("actionType debe ser Login o Busqueda");
            }
            UserId = userId;
            ActionType = actionType;
            SearchQuery = searchQuery;
            Timestamp = timestamp;
        }

        public LogEntry() { }
    }
}
