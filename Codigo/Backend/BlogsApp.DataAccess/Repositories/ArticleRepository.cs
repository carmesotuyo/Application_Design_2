using System;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
	public class ArticleRepository : IArticleRepository
	{
        private readonly DbSet<Article> articles;
        private readonly DbContext context;

        public ArticleRepository(DbContext context)
        {
            this.context = context;
            this.articles = context.Set<Article>();
        }

        //.../ARTICLE REPOSITORY CODE
    }
}

