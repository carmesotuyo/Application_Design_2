using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogsApp.DataAccess
{
	public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string directory = Directory.GetCurrentDirectory();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(directory)
                 .AddJsonFile("appsettings.json")
                 .Build();

                var connectionString = configuration.GetConnectionString(@"BlogsAppDBCarme");
                // var connectionString = configuration.GetConnectionString(@"BlogsAppDBFer");
                // var connectionString = configuration.GetConnectionString(@"BlogsAppDBGime");

                optionsBuilder.UseSqlServer(connectionString!);
            }
        }

    }
}

