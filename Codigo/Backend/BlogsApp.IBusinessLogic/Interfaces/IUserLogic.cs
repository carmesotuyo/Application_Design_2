using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        User? CreateUser(User? user);
        public User DeleteUser(User loggedUser, int UserId);
        public ICollection<User> GetUsersRanking(User loggedUser, DateTime dateFrom, DateTime dateTo, int? top);
        public User GetUserById(int userId);
        User? UpdateUser(User loggedUser, User user);
    }
}
