using System.Data;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class CommentLogic : ICommentLogic
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IOffensiveWordsValidator _offensiveWordsValidator;

        public CommentLogic(ICommentRepository commentRepository, IOffensiveWordsValidator offensiveWordsValidator)
        {
            _commentRepository = commentRepository;
            _offensiveWordsValidator = offensiveWordsValidator;
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
                List<string> offensiveWordsFound = _offensiveWordsValidator.reviewComment(comment);
                if (offensiveWordsFound.Count() > 0)
                {
                    comment.State = Domain.Enums.ContentState.InReview;
                    _offensiveWordsValidator.NotifyAdminsAndModerators();
                }

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
                comment.State = Domain.Enums.ContentState.Deleted;
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
                                        .Where(c => c.Article.UserId == loggedUser.Id &&
                                            (c.State == Domain.Enums.ContentState.Visible || c.State == Domain.Enums.ContentState.Edited) &&
                                            c.DateModified > lastLogout)
                                        .ToList();
            return comments;
        }

        private Func<Comment, bool> CommentById(int id)
        {
            return a => a.Id == id && a.DateDeleted == null && (a.State == Domain.Enums.ContentState.Visible || a.State == Domain.Enums.ContentState.Edited);
        }

        public Comment GetCommentById(int id)
        {
            return _commentRepository.Get(CommentById(id));
        }
    }
}