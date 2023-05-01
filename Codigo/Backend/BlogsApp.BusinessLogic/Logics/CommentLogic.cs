using System.Data;
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
            if (loggedUser.Blogger)
            {
                this._commentRepository.Add(comment);
                return comment;
            }

            throw new UnauthorizedAccessException("Sólo Bloggers pueden hacer comentarios");
        }

        public void DeleteComment(int commentId)
        {
            //throw new NotImplementedException();
        }
    }
}