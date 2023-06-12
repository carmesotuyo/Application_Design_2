using BlogsApp.BusinessLogic.Logics;
using BlogsApp.DataAccess;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlogsApp.Logging.DataAccess.Repositories;
using BlogsApp.Logging.Logic.Services;

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleLogic, ArticleLogic>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentLogic, CommentLogic>();
            services.AddScoped<ILogEntryRepository, LogEntryRepository>();
            services.AddScoped<ILoggerService, DbLoggerService>();
            services.AddScoped<IOffensiveWordRepository, OffensiveWordRepository>();
            services.AddScoped<IOffensiveWordsValidator, OffensiveWordsValidator>();
        }

        public void AddDbContextService()
        {
            services.AddDbContext<DbContext, Context>();
        }
    }
}

