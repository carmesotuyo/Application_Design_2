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
    }
}
