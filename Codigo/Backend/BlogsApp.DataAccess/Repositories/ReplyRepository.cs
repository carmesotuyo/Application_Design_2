using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;
using BlogsApp.DataAccess.Interfaces.Exceptions;

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
            throw new NotImplementedException();
        }

        public Reply Get(Func<Reply, bool> func)
        {
            throw new NotImplementedException();
        }

        public ICollection<Reply> GetAll(Func<Reply, bool> func)
        {
            throw new NotImplementedException();
        }

        public void Update(Reply value)
        {
            throw new NotImplementedException();
        }
    }
}