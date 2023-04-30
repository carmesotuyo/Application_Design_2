using System;
using System.Collections.Generic;
using BlogsApp.Domain.Entities;

namespace BlogsApp.IDataAccess.Interfaces
{
    public interface IReplyRepository : IRepository<Reply>
    {
        void Update(Reply value);
    }
}
