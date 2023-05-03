using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using System.Linq;

namespace BlogsApp.DataAccess.Repositories
{
    public class ReplyRepository : IReplyRepository
    {
        private DbContext Context { get; }

        public ReplyRepository(DbContext context)
        {
            Context = context;
        }

        public Reply Add(Reply value)
        {
            Context.Set<Reply>().Add(value);
            Context.SaveChanges();
            return value;
        }

        public bool Exists(Func<Reply, bool> func)
        {
            return Context.Set<Reply>().Where(func).Any();
        }

        public Reply Get(Func<Reply, bool> func)
        {
            Reply reply = Context.Set<Reply>().Include("User").Where(a => a.DateDeleted == null).FirstOrDefault(func);
            if (reply == null)
                throw new NotFoundDbException("No se encontró el reply");
            return reply;
        }

        public ICollection<Reply> GetAll(Func<Reply, bool> func)
        {
            ICollection<Reply> replies = Context.Set<Reply>().Include("User").Where(a => a.DateDeleted == null).Where(func).ToArray();
            if (replies.Count == 0)
                throw new NotFoundDbException("No se encontraron replies");
            return replies;
        }

        public void Update(Reply value)
        {
            bool exists = Context.Set<Reply>().Where(i => i.Id == value.Id).Any();
            if (!exists)
                throw new NotFoundDbException("No existe reply con ese Id");
            Reply original = Context.Set<Reply>().Find(value.Id);
            Context.Entry(original).CurrentValues.SetValues(value);
            Context.SaveChanges();
        }
    }
}