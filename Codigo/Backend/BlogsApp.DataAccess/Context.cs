using System;
using BlogsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogsApp.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Session> Sessions { get; set; }


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

                //var connectionString = configuration.GetConnectionString(@"BlogsAppDBCarme");
                //var connectionString = configuration.GetConnectionString(@"BlogsAppDBFer");
                var connectionString = configuration.GetConnectionString(@"BlogsAppDBGime");

                optionsBuilder.UseSqlServer(connectionString!);
            }
        }

    }
}

