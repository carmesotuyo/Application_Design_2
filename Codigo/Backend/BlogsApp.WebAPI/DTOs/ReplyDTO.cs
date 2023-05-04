using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class ReplyDTO
    {
        public int? Id { get; set; }
        public BasicUserDTO User { get; set; }
        public string Body { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }
    }

    public class BasicReplyDTO
    {
        public string Body { get; set; }
        public int CommentId { get; set; }
    }
}
