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
                Username = article.User.Username,
                Body = article.Body,
                Private = article.Private,
                Template = article.Template,
                Image = article.Image,
                UserId = article.UserId
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

        public static CompleteArticleDTO ToCompleteDto(Article article)
        {
            CompleteArticleDTO articleDto = new CompleteArticleDTO()
            {
                Id = article.Id,
                Name = article.Name,
                Username = article.User.Username,
                Body = article.Body,
                Private = article.Private,
                Template = article.Template,
                Image = article.Image
            };

            if (article.Comments!= null && article.Comments.Count > 0)
            {
                ICollection<CommentDTO> commentsDtos = CommentConverter.ToDtoList(article.Comments);
                articleDto.commentsDtos = commentsDtos;
            }
            return articleDto;
        }
    }
}
