using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;
using BlogsApp.DataAccess.Interfaces.Exceptions;

namespace BlogsApp.DataAccess.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private DbContext Context { get; }

        public ArticleRepository(DbContext context)
        {
            Context = context;
        }

        public Article Add(Article value)
        {
            bool exists = Context.Set<Article>().Where(i => i.Id == value.Id).Any();
            if (exists)
                throw new AlreadyExistsDbException();
            Context.Set<Article>().Add(value);
            Context.SaveChanges();
            return value;
        }

        public bool Exists(Func<Article, bool> func)
        {
            throw new NotImplementedException();
            //completar codigo
        }

        public Article Get(Func<Article, bool> func)
        {
            Article article = Context.Set<Article>().Include("User").Where(a => a.DateDeleted == null).FirstOrDefault(func);
            if (article == null)
                throw new NotFoundDbException();
            return article;
        }

        public ICollection<Article> GetAll(Func<Article, bool> func)
        {
            ICollection<Article> articles = Context.Set<Article>().Include("User").Where(a => a.DateDeleted != null).Where(func).ToArray();
            if (articles.Count == 0)
                throw new NotFoundDbException();
            return articles;
        }

        public void Update(Article value)
        {
            throw new NotImplementedException();
            //completar codigo
        }

    }
}

