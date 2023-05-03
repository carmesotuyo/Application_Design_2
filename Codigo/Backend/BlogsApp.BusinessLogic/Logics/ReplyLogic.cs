using System.Xml.Linq;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class ReplyLogic : IReplyLogic
    {
        private readonly IReplyRepository _replyRepository;

        public ReplyLogic(IReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        public Reply CreateReply(Reply reply, User loggedUser)
        {
            if (loggedUser.Blogger)
            {
                this._replyRepository.Add(reply);
                return reply;
            }

            throw new UnauthorizedAccessException("Sólo Bloggers pueden responder comentarios");
        }

        public void DeleteReply(int commentId, User loggedUser)
        {
            throw new NotImplementedException();
        }
    }
}
