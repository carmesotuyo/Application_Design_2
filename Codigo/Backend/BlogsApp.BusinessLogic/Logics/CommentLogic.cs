using System.Data;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class CommentLogic : ICommentLogic
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserLogic _userLogic;

        public CommentLogic(ICommentRepository commentRepository, IUserLogic userLogic)
        {
            _commentRepository = commentRepository;
            _userLogic = userLogic;
        }

        public Comment ReplyToComment(Comment parentComment, Comment newComment, User loggedUser)
        {
            if (loggedUser.Blogger)
            {
                Comment createdComment = this.CreateComment(newComment, loggedUser);
                parentComment.SubComments.Add(newComment);
                this._commentRepository.Update(parentComment);
                return createdComment;
            }

            throw new UnauthorizedAccessException("Sólo Bloggers pueden hacer comentarios");
        }

        public Comment CreateComment(Comment comment, User loggedUser)
        {
            if (loggedUser.Blogger)
            {
                this._commentRepository.Add(comment);
                loggedUser.Comments.Add(comment);
                this._userLogic.UpdateUser(loggedUser, loggedUser);
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
            return a => a.Id == id && a.DateDeleted == null;
        }

        public Comment GetCommentById(int id)
        {
            return _commentRepository.Get(CommentById(id));
        }
    }
}