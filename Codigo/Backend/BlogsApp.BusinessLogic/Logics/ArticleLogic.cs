using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class ArticleLogic : IArticleLogic
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleLogic(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public Article GetArticleById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Article> GetArticles(string? searchText)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Article> GetArticlesByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetStatsByYear(int year)
        {
            throw new NotImplementedException();
        }

        //...ARTICLE LOGIC CODE
    }
}

