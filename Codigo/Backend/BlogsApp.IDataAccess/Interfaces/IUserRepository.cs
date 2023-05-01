using System;
using System.Collections.Generic;
using BlogsApp.Domain.Entities;


namespace BlogsApp.IDataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public void InsertUser(User user);
        void Update(User value);
    }
}
