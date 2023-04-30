using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;

namespace BlogsApp.DataAccess.Repositories
{
    public class ReplyRepository : IReplyRepository
    {
        //private readonly DbSet<Reply> replies;
        private DbContext Context { get; }

        public ReplyRepository(DbContext context)
        {
            Context = context;
            //this.replies = context.Set<Reply>();
        }

        public Reply Add(Reply value)
        {
            throw new NotImplementedException();
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

        //.../replies REPOSITORY CODE
    }
}