namespace BlogsApp.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public User User { get; private set; }
        public Guid Token { get; private set; }
        public DateTime DateTimeLogin { get; private set; }
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
