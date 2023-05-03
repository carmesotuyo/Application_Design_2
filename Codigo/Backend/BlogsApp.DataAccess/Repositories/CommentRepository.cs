using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using System.Linq;

namespace BlogsApp.DataAccess.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private DbContext Context { get; }

        public CommentRepository(DbContext context)
        {
            Context = context;
        }

        public Comment Add(Comment value)
        {
            Context.Set<Comment>().Add(value);
            Context.SaveChanges();
            return value;
        }

        public bool Exists(Func<Comment, bool> func)
        {
            return Context.Set<Comment>().Where(func).Any();
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
    }
}
