using BlogsApp.BusinessLogic.Logics;
using BlogsApp.DataAccess;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentLogic, CommentLogic>();
            services.AddScoped<IReplyRepository, ReplyRepository>();
            services.AddScoped<IReplyLogic, ReplyLogic>();
        }

        public void AddDbContextService() //string connectionString
        {
            services.AddDbContext<DbContext, Context>(); //options => options.UseSqlServer(connectionString)
        }
    }
}

