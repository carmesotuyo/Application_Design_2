using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;

namespace BlogsApp.DataAccess.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        //private readonly DbSet<Comment> comments;
        private DbContext Context { get; }

        public CommentRepository(DbContext context)
        {
            Context = context;
            //this.comments = context.Set<Comment>();
        }

        public Comment Add(Comment value)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Func<Comment, bool> func)
        {
            throw new NotImplementedException();
        }

        public Comment Get(Func<Comment, bool> func)
        {
            throw new NotImplementedException();
        }

        public ICollection<Comment> GetAll(Func<Comment, bool> func)
        {
            throw new NotImplementedException();
        }

        public void Update(Comment value)
        {
            throw new NotImplementedException();
        }

        //.../Comments REPOSITORY CODE
    }
}
