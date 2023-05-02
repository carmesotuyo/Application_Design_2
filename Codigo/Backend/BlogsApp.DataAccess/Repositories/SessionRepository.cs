using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private DbContext Context { get; }

        public SessionRepository(Context context)
        {
            Context = context;
        }

        public Session Add(Session value)
        {
            bool exists = Context.Set<Session>().Any(s => s.User == value.User && s.DateTimeLogout == null);
            if (exists)
            {
                throw new AlreadyExistsDbException("El usuario ya tiene una sesión activa");
            }
            Context.Set<Session>().Add(value);
            Context.SaveChanges();
            return value;
        }

        public void Update(Session value)
        {
            Session original = Context.Set<Session>().Find(value.Id);
            if (original == null)
            {
                throw new NotFoundDbException("No se encuentra el id en la base de datos");
            }
            Context.Entry(original).CurrentValues.SetValues(value);
            Context.SaveChanges();
        }


        public Session Get(Func<Session, bool> func)
        {
            throw new NotImplementedException();
        }

        public ICollection<Session> GetAll(Func<Session, bool> func)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Func<Session, bool> func)
        {
            throw new NotImplementedException();
        }
    }
}