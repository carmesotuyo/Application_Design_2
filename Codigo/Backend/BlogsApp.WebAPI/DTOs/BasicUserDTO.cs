using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class BasicUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public BasicUserDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}
