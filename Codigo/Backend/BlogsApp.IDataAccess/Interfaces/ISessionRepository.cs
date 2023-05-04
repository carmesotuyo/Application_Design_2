using System;
using System.Collections.Generic;
using BlogsApp.Domain.Entities;

namespace BlogsApp.IDataAccess.Interfaces
{
    public interface ISessionRepository : IRepository<Session>
    {
        void Update(Session value);
    }
}
