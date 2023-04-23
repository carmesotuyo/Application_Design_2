namespace BlogsApp.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Blogger { get; set; }
        public bool Admin { get; set; }
        public DateTime? DateDeleted { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reply> Replies { get; set; }

        public User(string username, string password, string email, string name, string lastName, bool blogger, bool admin)    
        {
            Username = username;
            Password = password;
            Email = email;
            Name = name;
            LastName = lastName;
            Blogger = blogger;
            Admin = admin;
            Articles = new List<Article>();
            Comments = new List<Comment>();
            Replies = new List<Reply>();
        }

        public User() { }
    }
}
