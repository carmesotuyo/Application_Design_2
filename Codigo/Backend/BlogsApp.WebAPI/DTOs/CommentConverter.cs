using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;

namespace BlogsApp.WebAPI.DTOs
{
    public class CommentConverter
    {
        public static CommentDTO toDto(Comment comment)
        {
            CommentDTO commentDto = new CommentDTO()
            {
                Id = comment.Id,
                User = new BasicUserDTO(comment.User),
                ArticleId = comment.Article.Id,
                Body = comment.Body,
                DateCreated = comment.DateCreated,
                DateDeleted = comment.DateDeleted,
                SubComments = new List<CommentDTO>(),
                State = comment.State
            };

            foreach(Comment subcomment in comment.SubComments)
            {
                commentDto.SubComments.Add(toDto(subcomment));
            }


            return commentDto;
        }

        public static BasicCommentDTO toBasicDto(Comment comment)
        {
            BasicCommentDTO dto = new BasicCommentDTO()
            {
                Body = comment.Body,
                ArticleId = comment.Article.Id,
                State = comment.State
            };
            if (dto.State == Domain.Enums.ContentState.InReview && comment.OffensiveWords != null)
            {
                dto.Message = "Tu Comentario contiene palabras ofensivas, no se mostrará hasta que salga de revisión por un Moderador";
                dto.OffensiveWords = OffensiveWordsValidatorUtils.mapToStrings(comment.OffensiveWords);
            }
            return dto;
        }

        public static NotificationCommentDto toNotificationDto(Comment comment)
        {
            return new NotificationCommentDto()
            {
                Body = comment.Body,
                ArticleId = comment.Article.Id,
                ArticleName = comment.Article.Name,
                Username = comment.User.Name,
                CommentId = comment.Id
            };
        }

        public static Comment FromDto(BasicCommentDTO dto, User user, Article article)
        {
            return new Comment(user, dto.Body, article);
        }


        public static IList<CommentDTO> ToDtoList(ICollection<Comment> comments)
        {
            List<CommentDTO> commentsDtos = new List<CommentDTO>();
            foreach (Comment comment in comments)
            {
                commentsDtos.Add(toDto(comment));
            }
            return commentsDtos;
        }
    }
}
