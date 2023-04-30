namespace BlogsApp.Domain.Entities
{
    public class Comment
    {
        public int Id { get; private set; }
        public User User { get; private set; }
        public string Body { get; set; }
        public Article Article { get; private set; }
        public Reply? Reply { get; set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public Comment(User user, string body, Article article)
        {
            User = user;
            Body = body;
            Article = article;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        public Comment() { }
    }
}
