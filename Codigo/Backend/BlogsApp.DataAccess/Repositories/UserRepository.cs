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
            bool exists = Context.Set<User>().Where(i => i.Id == value.Id).Any();
            if (exists)
                throw new AlreadyExistsDbException("El Nombre de usuario ya est� en uso");
            Context.Set<User>().Add(value);
            Context.SaveChanges();
            return value;
        }

        public void Update(User value)
        {
            bool exists = Context.Set<User>().Where(i => i.Id == value.Id).Any();
            if (!exists)
                throw new NotFoundDbException("No se encuentra el id en la base de datos");
            User original = Context.Set<User>().Find(value.Id);
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

        public bool Exists(Func<User, bool> func)
        {
            return Context.Set<User>().Where(func).Any();
        }
    }
}
