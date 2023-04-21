using System;
using BlogsApp.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BlogsApp.DataAccess;

namespace BlogsApp.WebAPI
{
	public class Startup
	{
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // EN ESTE METODÓ SE REGISTRAN LOS SERVICIOS
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // REGISTRO EL CONTEXTO
            //services.AddDbContext<DbContext, Context>(o => o.UseSqlServer(Configuration.GetConnectionString("BlogsAppDBCarme")));
            // REGISTRO EL REPOSITORIO Y SU LÓGICA
            //services.AddScoped<IMoviesService, MoviesService>();
            //services.AddScoped<IMoviesManagment, MoviesManagment>();

            ServiceFactory factory = new ServiceFactory(services);
            factory.AddCustomServices();
            factory.AddDbContextService(this.Configuration.GetConnectionString("BlogsAppDBCarme"));
        }

        //A CHEQUEAR:
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

