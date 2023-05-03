using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class ArticleConverter
    {
        public static Article FromDto(BasicArticleDto dto, User user)
        {
            return new Article(dto.Name, dto.Body, dto.Template, user)
            {
                Private = dto.Private,
                Image = dto.Image
            };
        }
        public static BasicArticleDto ToDto(Article article)
        {
            return new BasicArticleDto
            {
                Id = article.Id,
                Name = article.Name,
                Body = article.Body,
                Private = article.Private,
                Template = article.Template,
                Image = article.Image
            };
        }

        public static IEnumerable<BasicArticleDto> ToDtoList(IEnumerable<Article> articles)
        {
            List<BasicArticleDto> basicArticleDtos = new List<BasicArticleDto>();
            foreach (Article article in articles)
            {
                basicArticleDtos.Add(ToDto(article));
            }
            return basicArticleDtos;
        }
    }
}
