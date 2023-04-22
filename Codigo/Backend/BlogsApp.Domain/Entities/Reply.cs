namespace BlogsApp.Domain.Entities
{
    public class Reply
    {
        public User User { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Boolean delete { get; set; }
        public Reply()
        {
        }
    }
}
