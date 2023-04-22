using System;
using BlogsApp.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BlogsApp.DataAccess;

namespace BlogsApp.WebAPI
{
	public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            ServiceFactory factory = new ServiceFactory(services);
            factory.AddCustomServices();
            factory.AddDbContextService(this.Configuration.GetConnectionString("BlogsAppDBCarme"));
        }

        public void Configure(IApplicationBuilder app) //, IWebHostEnvironment env
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

