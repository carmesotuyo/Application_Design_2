using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface ICommentLogic
    {
        Comment CreateComment(Comment comment, User loggedUser);
        void DeleteComment(int commentId, User loggedUser);
        IEnumerable<Comment> GetCommentsSince(User loggedUser, DateTime? lastLogout);
        Comment ReplyToComment(Comment parentComment, Comment newComment, User loggedUser);
        Comment GetCommentById(int id, User loggedUser);
        Comment UpdateComment(int id, Comment commentWithDataToUpdate, User loggedUser);
    }
}
