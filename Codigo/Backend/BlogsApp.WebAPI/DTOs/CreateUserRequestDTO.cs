using BlogsApp.Domain.Entities;
namespace BlogsApp.WebAPI.DTOs
{
    public class CreateUserRequestDTO
    {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public bool Blogger { get; set; }
            public bool Admin { get; set; }

        public CreateUserRequestDTO() { }

            public User TransformToUser()
            {
                return new User(
                    this.Username,
                    this.Password,
                    this.Email,
                    this.Name,
                    this.LastName,
                    this.Blogger,
                    this.Admin
                );
            }
        }
    }
