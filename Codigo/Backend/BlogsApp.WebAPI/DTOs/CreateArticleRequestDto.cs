using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class CreateArticleRequestDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool Private { get; set; }
        public int Template { get; set; }
        public string? Image { get; set; }
    }

    public class ArticleConverter
    {
        public static Article FromDto(CreateArticleRequestDto dto, User user)
        {
            return new Article(dto.Name, dto.Body, dto.Template, user)
            {
                Private = dto.Private,
                Image = dto.Image
            };
        }



        public static CreateArticleRequestDto ToDto(Article article)
        {
            return new CreateArticleRequestDto
            {
                Id = article.Id,
                Name = article.Name,
                Body = article.Body,
                Private = article.Private,
                Template = article.Template,
                Image = article.Image
            };
        }
    }

}
