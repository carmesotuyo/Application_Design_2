using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class CommentLogic : ICommentLogic
    {
        private readonly ICommentRepository _commentRepository;

        public CommentLogic(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Comment CreateComment(Comment comment, User loggedUser)
        {
            throw new NotImplementedException();
        }
    }
}