using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IUserLogic
    {
        public IEnumerable<User> GetUsers();
        User? InsertUser(User? movie);
    }
}
