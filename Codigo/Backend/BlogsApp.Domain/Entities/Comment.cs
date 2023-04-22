namespace BlogsApp.Domain.Entities
{
    public class Comment
    {
        public User User { get; set; }
        public string Body { get; set; }
        public Reply? Reply { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public Comment(User user, string body)
        {
            User = user;
            Body = body;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }
    }
}
