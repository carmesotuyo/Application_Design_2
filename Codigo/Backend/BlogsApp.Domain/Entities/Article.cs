namespace BlogsApp.Domain.Entities
{
    public class Article
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool Private { get; set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public User User { get; private set; }
        public ICollection<Comment>? Comments;
        public int Template { get; set; }
        public string? Image { get; set; }


        public Article(string name, string body, int template, User user)
        {
            Name = name;
            Body = body;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
            User = user;
            Comments = new List<Comment>();
            Template = template;
        }

        public Article() { }
    }
}

