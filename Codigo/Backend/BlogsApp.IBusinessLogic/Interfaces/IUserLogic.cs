using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        User? InsertUser(User? user);
        User? UpdateUser(User user);
        //public User DeleteUser(User user);
        //public User GetUserById(int id);
    }
}
