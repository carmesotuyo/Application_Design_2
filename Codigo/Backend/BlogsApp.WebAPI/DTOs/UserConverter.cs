using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class UserConverter
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                LastName = user.LastName,
                Blogger = user.Blogger,
                Admin = user.Admin,
                Moderador = user.Moderador,
                DateDeleted = user.DateDeleted
        };
        }

        public static IEnumerable<UserDto> ToDtoList(IEnumerable<User> users)
        {
            List<UserDto> userDtos = new List<UserDto>();
            foreach (User user in users)
            {
               userDtos.Add(ToDto(user));
            }
            return userDtos;
        }

    }
}
