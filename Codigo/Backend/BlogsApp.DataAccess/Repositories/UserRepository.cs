using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        //private readonly DbSet<User> users;
        private readonly DbContext context;

        public UserRepository(DbContext context)
        {
            this.context = context;
            //this.users = context.Set<User>();
        }

        //.../Users REPOSITORY CODE
    }
}
