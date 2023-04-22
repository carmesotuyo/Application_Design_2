using System;
using Microsoft.Extensions.DependencyInjection;
using BlogsApp.BusinessLogic;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.IBusinessLogic;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.Factory
{
    public class ServiceFactory
    {
        private readonly IServiceCollection services;

        public ServiceFactory(IServiceCollection services)
        {
            this.services = services;
        }

        public void AddCustomServices()
        {
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleLogic, ArticleLogic>();
        }

        public void AddDbContextService() //string connectionString
        {
            services.AddDbContext<DbContext, Context>(); //options => options.UseSqlServer(connectionString)
        }
    }
}

