using System.Data;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class CommentLogic : ICommentLogic
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IReplyLogic _replyLogic;

        public CommentLogic(ICommentRepository commentRepository, IReplyLogic replyLogic)
        {
            _commentRepository = commentRepository;
            _replyLogic = replyLogic;
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
                if(comment.Reply != null)
                {
                    _replyLogic.DeleteReply(comment.Reply.Id, loggedUser);
                }
                comment.DateDeleted = DateTime.Now;
                this._commentRepository.Update(comment);
            }
            else
            {
                throw new UnauthorizedAccessException("Sólo el creador del comentario puede eliminarlo");
            };
        }

        public IEnumerable<Comment> GetCommentsSince(DateTime? lastLogout)
        {
            throw new NotImplementedException();
        }

        private Func<Comment, bool> CommentById(int id)
        {
            return a => a.Id == id && a.DateDeleted != null;
        }
    }
}