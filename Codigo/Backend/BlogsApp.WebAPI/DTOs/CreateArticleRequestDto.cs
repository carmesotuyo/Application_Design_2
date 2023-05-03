using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class BasicArticleDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool Private { get; set; }
        public int Template { get; set; }
        public string? Image { get; set; }
    }
}
