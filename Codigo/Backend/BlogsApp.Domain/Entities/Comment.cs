using BlogsApp.Domain.Enums;
namespace BlogsApp.Domain.Entities
{
    public class Comment : Content
    {
        public Article Article { get; set; }
        public User User { get; set; }
        public ICollection<Comment> SubComments { get; set; }
        public bool isSubComment { get; set; }
        public ICollection<OffensiveWord> OffensiveWords { get; set; }

        public Comment(User user, string body, Article article)
        {
            User = user;
            Body = body;
            Article = article;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
            SubComments = new List<Comment>();
            OffensiveWords = new List<OffensiveWord>();
        }

        public Comment() { SubComments = new List<Comment>(); }
    }
}
