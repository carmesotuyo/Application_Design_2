using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IArticleLogic
    {
        public IEnumerable<Article> GetArticles();
    }
}

