namespace BlogsApp.Domain.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Blogger { get; set; }
        public bool Admin { get; set; }
        public DateTime? DateDeleted { get; set; }
        public List<Article> Articles { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Reply> Replies { get; set; }

        public User(string username, string pwd, string email, string name, string lastname, bool blogger, bool admin)    
        {
            Username = username;
            Password = pwd;
            Email = email;
            Name = name;
            LastName = lastname;
            Blogger = blogger;
            Admin = admin;
            Articles = new List<Article>();
            Comments = new List<Comment>();
            Replies = new List<Reply>();
        }
    }
}
