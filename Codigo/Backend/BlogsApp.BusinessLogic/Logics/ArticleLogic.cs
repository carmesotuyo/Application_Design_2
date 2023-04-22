using System;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
	public class ArticleLogic : IArticleLogic
	{
        private readonly IArticleRepository articleRepository;

        public ArticleLogic(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        //...ARTICLE LOGIC CODE
    }
}

