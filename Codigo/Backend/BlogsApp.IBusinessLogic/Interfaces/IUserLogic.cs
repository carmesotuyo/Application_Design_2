using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        User? CreateUser(User? user);
        public User DeleteUser(int UserId);
        public ICollection<User> GetUsersRanking(int? top);
        public User GetUserById(int userId);
        User? UpdateUser(User user);
    }
}
