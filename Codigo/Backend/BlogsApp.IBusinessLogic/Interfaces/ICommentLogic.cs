using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface ICommentLogic
    {
        Comment CreateComment(Comment comment, User loggedUser);
        void DeleteComment(int commentId);
        IEnumerable<Comment> GetCommentsSince(DateTime? lastLogout);
    }
}
