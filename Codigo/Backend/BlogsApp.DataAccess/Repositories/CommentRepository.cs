using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;
using BlogsApp.DataAccess.Interfaces.Exceptions;

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
            Comment comment = Context.Set<Comment>().Include(c => c.User).Include(c => c.Article).Include(c => c.OffensiveWords).Where(a => a.DateDeleted == null).FirstOrDefault(func);
            if (comment == null)
                throw new NotFoundDbException("No se encontraron comentarios");
            return comment;
        }

        public ICollection<Comment> GetAll(Func<Comment, bool> func)
        {
            ICollection<Comment> comments = Context.Set<Comment>().Include("User").Include("Article").Where(a => a.DateDeleted == null).Where(func).ToArray();
            
            return comments;
        }

        public void Update(Comment value)
        {
            bool exists = Context.Set<Comment>().Where(i => i.Id == value.Id).Any();
            if (!exists)
                throw new NotFoundDbException("No existe comentario con ese Id");
            Comment original = Context.Set<Comment>().Find(value.Id);
            Context.Entry(original).CurrentValues.SetValues(value);
            Context.SaveChanges();
        }
    }
}
