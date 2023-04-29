using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        //private readonly DbSet<User> users;
        private readonly Context context;

        public UserRepository(Context context)
        {
            this.context = context;
            //this.users = context.Set<User>();
        }

        public IEnumerable<User>? GetUsers()
        {
            return context.Users?.ToList();
        }

        public void InsertUser(User user)
        {
            context.Users?.Add(user);
            context.SaveChanges();
        }

        //.../Users REPOSITORY CODE
    }
}
