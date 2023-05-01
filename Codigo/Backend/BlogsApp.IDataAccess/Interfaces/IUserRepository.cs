using BlogsApp.Domain.Entities;

namespace BlogsApp.IDataAccess.Interfaces
{
    public interface IUserRepository
    {
        public void InsertUser(User user);
    }
}
