using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class UserRankingDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class UserRankingConverter
    {
        public static UserRankingDto ToRankingDto(User user)
        {
            return new UserRankingDto
            {
                Id = user.Id,
                Username = user.Username,
            };
        }

        public static ICollection<UserRankingDto> ToRankingList(ICollection<User> users)
        {
            ICollection<UserRankingDto> result = new List<UserRankingDto>();
            foreach(User user in users)
            {
                result.Add(ToRankingDto(user));
            }
            return result;
        }
    }
}
