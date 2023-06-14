using BlogsApp.Domain.Entities;
using BlogsApp.Domain.Enums;

namespace BlogsApp.WebAPI.DTOs
{
    public class BasicArticleDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public bool Private { get; set; }
        public int Template { get; set; }
        public string? Image { get; set; }
        public int UserId { get; set; }
        public ContentState State { get; set; }
        public string? Message { get; set; }
        public ICollection<string>? OffensiveWords { get; set; }

    }
}
