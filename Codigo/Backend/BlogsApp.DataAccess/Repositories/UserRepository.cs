using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        //private readonly DbSet<User> users;
        private DbContext Context { get; }

        public UserRepository(DbContext context)
        {
            Context = context;
            //this.users = context.Set<User>();
        }

        public void Update(User value)
        {
            throw new NotImplementedException();
        }

        public User Add(User value)
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

        //.../Users REPOSITORY CODE
    }
}
