using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class UpdateArticleRequestDTO
    {
        public string? Name { get; set; }
        public string? Body { get; set; }
        public bool? Private { get; set; }
        public int? Template { get; set; }
        public string? Image { get; set; }

        public Article ApplyChangesToArticle(Article article)
        {
            if (!string.IsNullOrEmpty(Name))
                article.Name = Name;

            if (!string.IsNullOrEmpty(Body))
                article.Body = Body;

            if (Private.HasValue)
                article.Private = Private.Value;

            if (Private.HasValue)
                article.Template = Template.Value;

            if (!string.IsNullOrEmpty(Image))
                article.Image = Image;

            return article;
        }
    }
}
