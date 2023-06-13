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

        public static User ToUser(UserDto userDto, int userId, User user)
        {
            User userDataToUpdate = new User();


            userDataToUpdate.Id = userId;

            if (!string.IsNullOrEmpty(userDto.Name)) { userDataToUpdate.Name = userDto.Name;  } else { userDataToUpdate.Name = user.Name; }

            if (!string.IsNullOrEmpty(userDto.Username)) { userDataToUpdate.Username = userDto.Username; } else { userDataToUpdate.Username = user.Username; }

            if (!string.IsNullOrEmpty(userDto.Password)) { userDataToUpdate.Password = userDto.Password; } else { userDataToUpdate.Password = user.Password; }

            if (!string.IsNullOrEmpty(userDto.Email)) { userDataToUpdate.Email = userDto.Email; } else { userDataToUpdate.Email = user.Email; }

            if (!string.IsNullOrEmpty(userDto.LastName)) { userDataToUpdate.LastName = userDto.LastName; } else { userDataToUpdate.LastName = user.LastName; }

            if (userDto.Blogger.HasValue) { userDataToUpdate.Blogger = userDto.Blogger.Value; } else { userDataToUpdate.Blogger = user.Blogger; }

            if (userDto.Admin.HasValue) { userDataToUpdate.Admin = userDto.Admin.Value; } else { userDataToUpdate.Admin = user.Admin; }

            if (userDto.Moderador.HasValue) { userDataToUpdate.Moderador = userDto.Moderador.Value; } else { userDataToUpdate.Moderador = user.Moderador; }

            if (userDto.DateDeleted.HasValue) { userDataToUpdate.DateDeleted = userDto.DateDeleted.Value; } else { userDataToUpdate.DateDeleted = user.DateDeleted; }

            return userDataToUpdate;
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
