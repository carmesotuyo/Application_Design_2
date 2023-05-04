using BlogsApp.Domain.Entities;
namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IReplyLogic
    {
        Reply CreateReply(Reply reply, User loggedUser);
        void DeleteReply(int commentId, User loggedUser);
    }
}
