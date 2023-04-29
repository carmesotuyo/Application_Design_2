using BlogsApp.Domain.Entities;

namespace BlogsApp.IDataAccess.Interfaces
{
    public interface IUserRepository
    {
        public IEnumerable<User>? GetUsers();
        void InsertUser(User user);
    }
}
