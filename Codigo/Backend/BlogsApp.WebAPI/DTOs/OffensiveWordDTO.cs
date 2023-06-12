using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
	public class OffensiveWordDTO
	{
		public string word { get; set; }
	}

	public enum typesOfContent
	{
		Article,
		Comment
	}

	public class ContentDTO
	{
		public int id { get; set; }
		public typesOfContent type { get; set; }
		public string body { get; set; }
		public string? articleName { get; set; }
	}

	public class ContentConverter
	{
		public static ContentDTO ArticleToContentDto(Article article)
		{
            return new ContentDTO
            {
                id = article.Id,
				type = typesOfContent.Article,
				articleName = article.Name,
				body = article.Body
            };
        }

		public static ICollection<ContentDTO> ArticlesToContentList(ICollection<Article> articles)
		{
            List<ContentDTO> content = new List<ContentDTO>();
            foreach (Article article in articles)
            {
                content.Add(ArticleToContentDto(article));
            }
            return content;
        }

        public static ContentDTO CommentToContentDto(Comment comment)
        {
            return new ContentDTO
            {
                id = comment.Id,
                type = typesOfContent.Comment,
                body = comment.Body
            };
        }

        public static ICollection<ContentDTO> CommentsToContentList(ICollection<Comment> comments)
        {
            List<ContentDTO> content = new List<ContentDTO>();
            foreach (Comment comment in comments)
            {
                content.Add(CommentToContentDto(comment));
            }
            return content;
        }
    }
}

