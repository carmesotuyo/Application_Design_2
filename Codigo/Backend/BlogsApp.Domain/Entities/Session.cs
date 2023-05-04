namespace BlogsApp.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Guid Token { get; set; }
        public DateTime DateTimeLogin { get; set; }
        public DateTime? DateTimeLogout { get; set; }

        public Session(User user, Guid token)
        {
            User = user;
            Token = token;
            DateTimeLogin = DateTime.Now;
        }

        public Session() { }
    }
}
