using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Blogger { get; set; }
        public bool Admin { get; set; }
        public bool Moderador { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
