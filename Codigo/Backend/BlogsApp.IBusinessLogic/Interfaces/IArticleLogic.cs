using BlogsApp.Domain.Entities;
namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IArticleLogic
    {
        Article GetArticleById(int id);
        IEnumerable<Article> GetArticles(string? search);
        IEnumerable<Article> GetArticlesByUser(int userId);
        IEnumerable<int> GetStatsByYear(int year);
    }
}

