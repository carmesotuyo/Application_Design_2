using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface ICommentLogic
    {
        Comment CreateComment(Comment comment, User loggedUser);
    }
}
