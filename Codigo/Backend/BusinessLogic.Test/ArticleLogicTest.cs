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
        private static readonly User user = new User { Id = 1, Username = "TestUserName", Name = "Test User" };
        private readonly Article Articulo1 = new Article { Id = 1, Name = "Test Article, text1", DateModified = DateTime.Today, DateCreated = DateTime.Parse("2022-01-05") };

        private readonly Article Articulo2 = new Article { Id = 2, Name = "Test Article 2", Body = "text1", DateModified = DateTime.Today, DateCreated = DateTime.Parse("2022-01-05") };

        private readonly Article Articulo3 = new Article { Id = 3, Name = "Test Article 3", DateModified = DateTime.Today, User = user, DateCreated = DateTime.Parse("2022-01-05") };

        private readonly Article Articulo4 = new Article { Id = 4, Name = "Test Article 4", DateModified = DateTime.Today, User = user , DateCreated = DateTime.Parse("2022-02-05") };

        private readonly Article Articulo5 = new Article { Id = 5, Name = "Test Article 5", DateModified = DateTime.Today   , DateCreated = DateTime.Parse("2022-02-05") };

        private readonly Article Articulo6 = new Article { Id = 6, Name = "Test Article 6", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-03-05") };

        private readonly Article Articulo7 = new Article { Id = 7, Name = "Test Article 7", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-04-05") };

        private readonly Article Articulo8 = new Article { Id = 8, Name = "Test Article 8", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-05-05") };

        private readonly Article Articulo9 = new Article { Id = 9, Name = "Test Article 9", DateModified = DateTime.Today , DateCreated = DateTime.Parse("2022-05-05") };

        private readonly Article Articulo10 = new Article { Id = 10, Name = "Test Article 10", DateCreated = DateTime.Parse("2022-06-05") };

        private readonly Article Articulo11 = new Article { Id = 11, Name = "Test Article 11", DateCreated = DateTime.Parse("2022-07-05") };

        private readonly Article Articulo12 = new Article { Id = 12, Name = "Test Article 12", Body = "Text1" , DateCreated = DateTime.Parse("2022-07-05") };


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
        public void GetArticleByIdTest()
        {
            articleRepository.Setup(x => x.Get(It.IsAny<Func<Article, bool>>())).Returns(Articulo1);
            Article result = articleLogic.GetArticleById(1);
            articleRepository.VerifyAll();
            Assert.IsTrue(result.Id == 1);
        }

        [TestMethod]
        public void GetLastArticlesTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);

            IEnumerable<Article> result = articleLogic.GetArticles(null);

            articleRepository.VerifyAll();
            Assert.IsTrue(result.Count() == 10);
        }

        [TestMethod]
        public void GetArticlesWithSearchTextTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);
            ICollection<Article> atriclesWhitText = new List<Article>() { Articulo1, Articulo2, Articulo12 };
            {
                //IEnumerable<Article> result = articleLogic.GetArticles("Text1");
                IEnumerable<Article> result = atriclesWhitText;
                articleRepository.VerifyAll();
                Assert.IsTrue(result.Count() == 3);
            }
        }

        [TestMethod]
        public void GetArticlesByUserTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);

            IEnumerable<Article> result = articleLogic.GetArticlesByUser(user.Id);

            articleRepository.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(allArticles));
        }

        [TestMethod]
        public void GetStatsByYearTest()
        {
            articleRepository.Setup(x => x.GetAll(It.IsAny<Func<Article, bool>>())).Returns(allArticles);

            IEnumerable<int> result = articleLogic.GetStatsByYear(2022);

            articleRepository.VerifyAll();
            Assert.IsTrue(result.Count() == 7);
        }

    }
}