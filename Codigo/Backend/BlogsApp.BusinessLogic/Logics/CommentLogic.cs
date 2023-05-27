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

        public Comment ReplyToComment(Comment parentComment, Comment newComment, User loggedUser)
        {
            if (loggedUser.Blogger)
            {
                parentComment.SubComments.Add(newComment);
                this._commentRepository.Update(parentComment);
                return this.CreateComment(newComment, loggedUser);
            }

            throw new UnauthorizedAccessException("Sólo Bloggers pueden hacer comentarios");
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

        public void DeleteComment(int commentId, User loggedUser)
        {
            Comment comment = _commentRepository.Get(CommentById(commentId));
            if (loggedUser.Id == comment.User.Id)
            {
                comment.DateDeleted = DateTime.Now;
                this._commentRepository.Update(comment);
            }
            else
            {
                throw new UnauthorizedAccessException("Sólo el creador del comentario puede eliminarlo");
            };
        }

        public IEnumerable<Comment> GetCommentsSince(User loggedUser, DateTime? lastLogout)
        {
            List<Comment> comments = _commentRepository.GetAll(c => c.DateDeleted == null)
                                        .Where(c => c.Article.UserId == loggedUser.Id && c.DateModified > lastLogout)
                                        .ToList();
            return comments;
        }

        private Func<Comment, bool> CommentById(int id)
        {
            return a => a.Id == id && a.DateDeleted != null;
        }
    }
}