using BlogsApp.Domain.Entities;
namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IArticleLogic
    {
        Article GetArticleById(int id, User loggedUser);
        IEnumerable<Article> GetArticles(User loggedUser, string? search);
        IEnumerable<Article> GetArticlesByUser(int userId, User loggedUser);
        IDictionary<string, int> GetStatsByYear(int year, User loggedUser);
        void DeleteArticle(int articleId, User loggedUser);
        Article CreateArticle(Article article, User loggedUser);
        Article UpdateArticle(int articleId, Article article, User loggedUser);
    }
}

