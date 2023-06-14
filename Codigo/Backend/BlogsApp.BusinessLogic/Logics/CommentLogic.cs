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
                    comment.HadOffensiveWords = true;
                    comment.OffensiveWords = _offensiveWordsValidator.mapToOffensiveWordsType(offensiveWordsFound);
                    _offensiveWordsValidator.NotifyAdminsAndModerators();
                }

                this._commentRepository.Add(comment);
                return comment;
            }

            throw new UnauthorizedAccessException("Sólo Bloggers pueden hacer comentarios");
        }

        public void DeleteComment(int commentId, User loggedUser)
        {
            Comment comment = _commentRepository.Get(CommentById(commentId, loggedUser));
            if (loggedUser.Id == comment.User.Id || loggedUser.Admin || loggedUser.Moderador)
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

        public Comment GetCommentById(int id, User loggedUser)
        {
            return _commentRepository.Get(CommentById(id, loggedUser));
        }

        public Comment UpdateComment(int id, Comment commentWithDataToUpdate, User loggedUser)
        {
            AuthorizedUser(loggedUser);

            Comment comment = _commentRepository.Get(CommentById(id, loggedUser));

            List<string> offensiveWordsFound = _offensiveWordsValidator.reviewComment(commentWithDataToUpdate);
            if (offensiveWordsFound.Count() > 0)
            {
                comment.State = Domain.Enums.ContentState.InReview;
                comment.HadOffensiveWords = true;
                _offensiveWordsValidator.NotifyAdminsAndModerators();
            }
            else if (comment.State == Domain.Enums.ContentState.InReview || comment.State == Domain.Enums.ContentState.Visible)
            {
                // si un contenido entra acá es porque o estaba en revisión y fue editado (se le quitaron las palabras ofensivas en la edición), o estaba publicado normal y fue editado y la edición no cuenta con palabras ofensivas
                comment.State = Domain.Enums.ContentState.Edited;
            }

            comment.Body = commentWithDataToUpdate.Body;
            comment.DateModified = DateTime.Now;

            // si no se encontraron palabras ofensivas la lista se guarda vacia, de lo contrario se actualizan las encontradas:
            comment.OffensiveWords = _offensiveWordsValidator.mapToOffensiveWordsType(offensiveWordsFound);
            this._commentRepository.Update(comment);
            return comment;
        }

        private Func<Comment, bool> CommentById(int id, User loggedUser)
        {
            return a => a.Id == id && a.DateDeleted == null &&
                (a.State == Domain.Enums.ContentState.Visible || a.State == Domain.Enums.ContentState.Edited ||
                (a.State == Domain.Enums.ContentState.InReview && (loggedUser.Moderador || loggedUser.Admin || a.User.Id == loggedUser.Id)));
        }

        private void AuthorizedUser(User loggedUser)
        {
            if (!loggedUser.Admin && !loggedUser.Moderador) throw new UnauthorizedAccessException("Sólo un moderador puede modificar el comentario");
        }

        public Comment ApproveComment(int id, User loggedUser)
        {
            if (loggedUser.Admin || loggedUser.Moderador)
            {
                Comment foundComment = GetCommentById(id, loggedUser);
                foundComment.State = Domain.Enums.ContentState.Visible;
                return UpdateComment(id, foundComment, loggedUser);
            }
            else
            {
                throw new UnauthorizedAccessException("Sólo administradores o moderadores pueden aprobar contenido");
            }
        }
    }
}