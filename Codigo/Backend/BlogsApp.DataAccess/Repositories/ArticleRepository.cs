using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;

namespace BlogsApp.DataAccess.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        //private readonly DbSet<Article> articles;
        private DbContext Context { get; }

        public ArticleRepository(DbContext context)
        {
            Context = context;
            //this.articles = context.Set<Article>();
        }

        public Article Add(Article value)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Func<Article, bool> func)
        {
            throw new NotImplementedException();
        }

        public Article Get(Func<Article, bool> func)
        {
            throw new NotImplementedException();
        }

        public ICollection<Article> GetAll(Func<Article, bool> func)
        {
            throw new NotImplementedException();
        }

        public void Update(Article value)
        {
            throw new NotImplementedException();
        }



        //.../ARTICLE REPOSITORY CODE
    }
}

