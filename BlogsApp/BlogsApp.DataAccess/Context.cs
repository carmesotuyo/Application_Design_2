using System;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess
{
	public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) { }
    }
}

