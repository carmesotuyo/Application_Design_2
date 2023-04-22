namespace BlogsApp.Domain.Entities
{
    public class Session
    {
        public User user { get; set; }
        public string token { get; set; }
        public DateTime dateTimeLogin { get; set; }
        public DateTime dateTimeLogout { get; set; }
        public Session()
        {
        }
    }
}
