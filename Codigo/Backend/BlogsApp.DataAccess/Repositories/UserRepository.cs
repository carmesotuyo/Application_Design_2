using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.DataAccess;

namespace BlogsApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        //private readonly DbSet<User> users;

        private readonly Context context;

        public UserRepository(Context context)
        {
            context = context;
            //this.users = context.Set<User>();
        }

        public User Add(User user)
        {
            context.Users?.Add(user);
            context.SaveChanges();
            return user;
        }

        public void Update(User value)
        {
            throw new NotImplementedException();
        }

        public User Get(Func<User, bool> func)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAll(Func<User, bool> func)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Func<User, bool> func)
        {
            throw new NotImplementedException();
        }
    }
}
