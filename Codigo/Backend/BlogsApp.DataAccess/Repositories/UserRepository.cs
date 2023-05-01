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
            this.context = context;
            //this.users = context.Set<User>();
        }

        public void InsertUser(User user)
        {
            context.Users?.Add(user);
            context.SaveChanges();
        }

        //.../Users REPOSITORY CODE
    }
}
