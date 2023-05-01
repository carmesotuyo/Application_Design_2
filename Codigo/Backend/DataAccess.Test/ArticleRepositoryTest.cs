using System.Collections.Generic;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogsApp.Domain;
using System.Linq.Expressions;
using BlogsApp.DataAccess;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.DataAccess.Interfaces.Exceptions;


using Microsoft.EntityFrameworkCore;

namespace BlogsApp.BusinessLogic.Tests
{
    [TestClass]
    public class ArticleRepositoryTests
    {
        private Article anArticle;
        private ICollection<Article> articles;
        private Func<Article, bool> getById;
        private string anotherName;

        [TestInitialize]
        public void TestInitialize()
        {
            anArticle = new Article()
            {
                Id = 1,
                Name = "Test Article",
                Body = "Test body",
                Private = false,
                DateModified = DateTime.Now,
                Template = 1,
                Image = null,
                DateDeleted = null
            };
            articles = new List<Article>() { anArticle };
            anotherName = "otherName";
        }

        [TestMethod]
        public void AddArticleOk()
        {
            var context = CreateContext();
            IArticleRepository articleRepository = new ArticleRepository(context);

            articleRepository.Add(anArticle);
            Article articleInDb = context.Articles.Where(a => a.Id == anArticle.Id).AsNoTracking().FirstOrDefault();

            Assert.IsNotNull(articleInDb);
            Assert.AreEqual(anArticle.Id, articleInDb.Id);
        }

        [TestMethod]
        public void AddArticleFail()
        {
            IArticleRepository articleRepository = CreateRepositoryWithArticle();

            Assert.ThrowsException<AlreadyExistsDbException>(() => articleRepository.Add(anArticle));
        }
        [TestMethod]
        public void GetArticleOk()
        {
            IArticleRepository articleRepository = CreateRepositoryWithArticle();
            getById = GetArticleById(anArticle.Id);

            Article articleInDb = articleRepository.Get(getById);

            Assert.IsNotNull(articleInDb);
            Assert.AreEqual(anArticle.Id, articleInDb.Id);
        }

        [TestMethod]
        public void GetArticleFail()
        {
            var context = CreateContext();
            IArticleRepository articleRepository = new ArticleRepository(context);
            getById = GetArticleById(anArticle.Id);

            Assert.ThrowsException<NotFoundDbException>(() => articleRepository.Get(getById));
        }

        private Context GetMemoryContext(string nameBd)
        {
            var builder = new DbContextOptionsBuilder<Context>();
            return new Context(GetMemoryConfig(builder, nameBd));
        }
        private DbContextOptions GetMemoryConfig(DbContextOptionsBuilder builder, string nameBd)
        {
            builder.UseInMemoryDatabase(nameBd);
            return builder.Options;
        }
        private IArticleRepository CreateRepositoryWithArticle()
        {
            var context = CreateContext();
            context.Articles.Add(anArticle);
            context.SaveChanges();

            IArticleRepository repository = new ArticleRepository(context);
            return repository;
        }
        private Context CreateContext()
        {
            Context context = GetMemoryContext("MemoryTestArticleDB");
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }
        private Func<Article, bool> GetArticleById(int Id)
        {
            return a => a.Id == Id;
        }
    }
}