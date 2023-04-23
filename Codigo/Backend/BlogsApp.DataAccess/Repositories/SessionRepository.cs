using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        //private readonly DbSet<Session> sessions;
        private readonly DbContext context;

        public SessionRepository(DbContext context)
        {
            this.context = context;
            //this.sessions = context.Set<Session>();
        }

        //.../sessions REPOSITORY CODE
    }
}