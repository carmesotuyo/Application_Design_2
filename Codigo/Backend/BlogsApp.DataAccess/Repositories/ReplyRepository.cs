using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogsApp.DataAccess.Repositories
{
    public class ReplyRepository : IReplyRepository
    {
        //private readonly DbSet<Reply> replies;
        private readonly DbContext context;

        public ReplyRepository(DbContext context)
        {
            this.context = context;
            //this.replies = context.Set<Reply>();
        }

        //.../replies REPOSITORY CODE
    }
}