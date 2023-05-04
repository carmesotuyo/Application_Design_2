using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class CommentConverter
    {
        public static CommentDTO toDto(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                User = new BasicUserDTO(comment.User),
                ArticleId = comment.Article.Id,
                Body = comment.Body,
                Reply = ReplyConverter.toDto(comment.Reply, comment.Reply.User),
                DateCreated = comment.DateCreated,
                DateDeleted = comment.DateDeleted
            };
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
