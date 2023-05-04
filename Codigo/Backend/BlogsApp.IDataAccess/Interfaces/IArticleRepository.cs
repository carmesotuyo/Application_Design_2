using BlogsApp.Domain.Entities;
using System;
using System.Collections.Generic;
namespace BlogsApp.IDataAccess.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        void Update(Article value);
    }
}

