namespace BlogsApp.Domain.Entities
{
    public class User
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public Boolean blogger { get; set; }
        public Boolean admin { get; set; }
        public Boolean delete { get; set; }
        public List<Article> articles { get; set; }
        public List<Comment> comments { get; set; }
        public List<Reply> replies { get; set; }

        public User()
        {
        }
    }
}
