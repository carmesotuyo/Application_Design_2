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

        //...Reply LOGIC CODE
    }
}
