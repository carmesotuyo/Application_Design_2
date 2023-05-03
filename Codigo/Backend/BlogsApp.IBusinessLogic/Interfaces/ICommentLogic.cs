using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface ICommentLogic
    {
        Comment CreateComment(Comment comment, User loggedUser);
        void DeleteComment(int commentId, User loggedUser);
        IEnumerable<Comment> GetCommentsSince(User loggedUser, DateTime? lastLogout);
        Reply AddReply(Comment comment, Reply reply, User loggedUser);
    }
}
