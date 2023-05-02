using BlogsApp.Domain.Entities;
namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IReplyLogic
    {
        Reply CreateReply(Reply reply, User loggedUser);
    }
}
