using BlogsApp.Domain.Enums;

namespace BlogsApp.WebAPI.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public BasicUserDTO User { get; set; }
        public int ArticleId { get; set; }
        public string Body { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public ICollection<CommentDTO> SubComments { get; set; }
        public ContentState State { get; set; }

    }

    public class BasicCommentDTO
    {
        public string Body { get; set; }
        public int ArticleId { get; set; }
        public ContentState State { get; set; }
        public string? Message { get; set; }
        public ICollection<string>? OffensiveWords { get; set; }
    }

    public class NotificationCommentDto
    {
        public string Body { get; set; }
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public string Username { get; set; }
        public int CommentId { get; set; }
    }

}
