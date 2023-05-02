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

        public void Update(Session value)
        {
            throw new NotImplementedException();
        }

        public Session Add(Session value)
        {
            throw new NotImplementedException();
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

        //.../sessions REPOSITORY CODE
    }
}