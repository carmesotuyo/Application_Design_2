using BlogsApp.Domain.Enums;

namespace BlogsApp.WebAPI.DTOs
{
    public class CompleteArticleDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public ICollection<CommentDTO> commentsDtos { get; set; }
        public bool Private { get; set; }
        public int Template { get; set; }
        public string? Image { get; set; }
        public ContentState State { get; set; }
    }
}
