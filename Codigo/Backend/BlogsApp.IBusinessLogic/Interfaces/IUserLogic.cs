using BlogsApp.Domain.Entities;
using System;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        User? CreateUser(User? user);
        User DeleteUser(User loggedUser, int UserId);
        ICollection<User> GetUsersRanking(User loggedUser, DateTime dateFrom, DateTime dateTo, int? top, bool? offensiveContent);
        User GetUserById(int userId);
        IEnumerable<User> GetUsers(User loggedUser);
        User? UpdateUser(User loggedUser, User userWithDataToUpdate);
        bool IsAdmin(User loggedUser);
        bool IsModerator(User loggedUser);
        bool IsBlogger(User loggedUser);

    }
}
