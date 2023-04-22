namespace BlogsApp.Domain.Entities
{
    public class Session
    {
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime DateTimeLogin { get; set; }
        public DateTime? DateTimeLogout { get; set; }

        public Session(User user, string token)
        {
            User = user;
            Token = token;
            DateTimeLogin = DateTime.Now;
        }
    }
}
