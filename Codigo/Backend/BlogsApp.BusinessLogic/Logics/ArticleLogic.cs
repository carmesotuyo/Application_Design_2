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

        public IEnumerable<Article> GetArticles()
        {
            throw new NotImplementedException();
        }

        //...ARTICLE LOGIC CODE
    }
}

