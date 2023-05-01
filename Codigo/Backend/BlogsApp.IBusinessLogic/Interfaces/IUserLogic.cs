using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        User? CreateUser(User? user);
        User? UpdateUser(int userId, User anUser);
        public User DeleteUser(int UserId);
        public ICollection<User> GetUsersRanking(int? top);
        public User GetUserById(int userId);
    }
}
