using System;
using System.Collections.Generic;
using BlogsApp.Domain.Entities;

namespace BlogsApp.IDataAccess.Interfaces
{
    public interface ICommentRepository  : IRepository<Comment>
    {
        void Update(Comment value);
    }
}
