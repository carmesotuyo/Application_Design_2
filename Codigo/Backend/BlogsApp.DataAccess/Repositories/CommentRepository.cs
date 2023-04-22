using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        //private readonly DbSet<Comment> comments;
        private readonly DbContext context;

        public CommentRepository(DbContext context)
        {
            this.context = context;
            //this.comments = context.Set<Comment>();
        }

        //.../Comments REPOSITORY CODE
    }
}
