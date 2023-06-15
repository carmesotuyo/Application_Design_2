using System;
using BlogsApp.Domain.Enums;

namespace BlogsApp.Domain.Entities
{
	public abstract class Content
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public ContentState State { get; set; }
        public bool HadOffensiveWords { get; set; }
    }
}

