using System;
namespace BlogsApp.Domain.Entities
{
    public class OffensiveWord
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public ICollection<Article> articlesContainingWord { get; set; }
        public ICollection<Comment> commentsContainingWord { get; set; }
    }
}

