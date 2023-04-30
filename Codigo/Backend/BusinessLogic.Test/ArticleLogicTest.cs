using System.Collections.Generic;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Domain;

namespace BlogsApp.BusinessLogic.Tests
{
    [TestClass]
    public class ArticleLogicTests
    {
        private readonly Article Articulo1 = new Article { Id = 1, Name = "Test Article, text1", DateModified = DateTime.Today};

        private readonly Article Articulo2 = new Article { Id = 2, Name = "Test Article 2", Body = "text1", DateModified = DateTime.Today };

        private readonly Article Articulo3 = new Article { Id = 3, Name = "Test Article 3", DateModified = DateTime.Today };

        private readonly Article Articulo4 = new Article { Id = 4, Name = "Test Article 4" , DateModified = DateTime.Today };

        private readonly Article Articulo5 = new Article { Id = 5, Name = "Test Article 5" , DateModified = DateTime.Today };

        private readonly Article Articulo6 = new Article { Id = 6, Name = "Test Article 6" , DateModified = DateTime.Today };

        private readonly Article Articulo7 = new Article { Id = 7, Name = "Test Article 7" , DateModified = DateTime.Today };

        private readonly Article Articulo8 = new Article { Id = 8, Name = "Test Article 8" , DateModified = DateTime.Today };

        private readonly Article Articulo9 = new Article { Id = 9, Name = "Test Article 9" , DateModified = DateTime.Today };

        private readonly Article Articulo10 = new Article { Id = 10, Name = "Test Article 10"};

        private readonly Article Articulo11 = new Article { Id = 11, Name = "Test Article 11" };

        private readonly Article Articulo12 = new Article { Id = 12, Name = "Test Article 12", Body = "Text1" };


        private Mock<IArticleRepository> articleRepository;
        private IArticleLogic articleLogic;
        private ICollection<Article> allArticles;

        [TestInitialize]
        public void TestInitialize()
        {
            articleRepository = new Mock<IArticleRepository>(MockBehavior.Default);
            articleLogic = new ArticleLogic(articleRepository.Object);
            allArticles = new List<Article>() { Articulo1, Articulo2, Articulo3, Articulo4, Articulo5, Articulo6, Articulo7, Articulo8, Articulo9, Articulo10, Articulo11, Articulo12 };
        }

        [TestMethod]
        public void GetLastArticlesTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);

            IEnumerable<Article> result = articleLogic.GetArticles(null);

            articleRepository.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(allArticles));
        }

        [TestMethod]
        public void GetArticlesWithSearchTextTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);
            IEnumerable<Article> result = articleLogic.GetArticles("Test");
            articleRepository.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(allArticles));
        }



       

        
    }
}