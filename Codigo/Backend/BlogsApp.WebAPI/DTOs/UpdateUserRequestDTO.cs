using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class UpdateUserRequestDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public bool? Blogger { get; set; }
        public bool? Admin { get; set; }
        public bool? Moderador { get; set; }

        public User ApplyChangesToUser(User user)
        {
            if (!string.IsNullOrEmpty(Username))
                user.Username = Username;

            if (!string.IsNullOrEmpty(Password))
                user.Password = Password;

            if (!string.IsNullOrEmpty(Email))
                user.Email = Email;

            if (!string.IsNullOrEmpty(Name))
                user.Name = Name;

            if (!string.IsNullOrEmpty(LastName))
                user.LastName = LastName;

            if (Blogger.HasValue)
                user.Blogger = Blogger.Value;

            if (Admin.HasValue)
                user.Admin = Admin.Value;

            if (Moderador.HasValue)
                user.Moderador = Moderador.Value;

            return user;
        }
    }

}
