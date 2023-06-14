using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.DataAccess;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using System.Linq;

namespace BlogsApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DbContext Context { get; }

        public UserRepository(Context context)
        {
            Context = context;
        }

        public User Add(User value)
        {
            bool exists = Context.Set<User>().Any(i => i.Username == value.Username);
            if (exists)
            {
                throw new AlreadyExistsDbException("El Nombre de usuario ya está en uso");
            }
            Context.Set<User>().Add(value);
            Context.SaveChanges();
            return value;
        }

        public void Update(User value)
        {
            User original = Context.Set<User>().Find(value.Id);
            if (original == null)
            {
                throw new NotFoundDbException("No se encuentra el id en la base de datos");
            }
            Context.Entry(original).CurrentValues.SetValues(value);
            Context.SaveChanges();
        }

        public User Get(Func<User, bool> func)
        {
            User user = Context.Set<User>().Where(a => a.DateDeleted == null).FirstOrDefault(func);
            if (user == null)
                throw new NotFoundDbException("No se encontraron usuarios");
            return user;
        }

        public ICollection<User> GetAll(Func<User, bool> func)
        {
            ICollection<User> users = Context.Set<User>().Where(func).ToArray();
            if (users.Count == 0)
                throw new NotFoundDbException("No se encontraron usuarios");
            return users;
        }


        public int GetUserContentCount(Func<User, bool> userFunc, Func<Content, bool> contentFunc)
        {
            User user = Context.Set<User>().Where(userFunc).FirstOrDefault();
            if (user == null)
                throw new NotFoundDbException("No se encontró el usuario");

            int articlesCount = Context.Entry(user).Collection(u => u.Articles).Query().Where(contentFunc).Count();
            int commentsCount = Context.Entry(user).Collection(u => u.Comments).Query().Where(contentFunc).Count();
            return articlesCount + commentsCount;
        }


        public bool Exists(Func<User, bool> func)
        {
            return Context.Set<User>().Where(func).Any();
        }
    }
}
