namespace BlogsApp.Domain.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool Private { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Nullable<DateTime> DateDeleted { get; set; }
        public User User { get; set; }
        public List<Comment> Comments;
        public int Template { get; set; }
        public Nullable<string> Image { get; set; }


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
    }
}

