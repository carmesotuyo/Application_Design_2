using System;
using BlogsApp.BusinessLogic;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.IBusinessLogic;
using BlogsApp.IBusinessLogic.Interfaces;

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
            services.AddScoped<IArticleLogic, ArticleLogic>();
        }
    }
}

