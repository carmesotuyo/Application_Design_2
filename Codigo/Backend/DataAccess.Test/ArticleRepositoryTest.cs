using BlogsApp.DataAccess;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.DataAccess.Repositories;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Test
{
    [TestClass]
    public class ArticleRepositoryTests
    {
        private Article anArticle;
        private ICollection<Article> articles;
        private Func<Article, bool> getById;
        private string anotherName;

        private Context _dbContext;
        private ArticleRepository _articleRepository;
        private UserRepository _userRepository;
        private User _testUser;
        private Article _testArticle;

        [TestInitialize]
        public void TestInit()
        {
            var options = new DbContextOptionsBuilder<Context>()
                        .UseInMemoryDatabase(databaseName: "ArticleDb")
                        .Options;
            _dbContext = new Context(options);
            _articleRepository = new ArticleRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);

            _testUser = new User("username", "password", "email@.com", "name", "last_name", false, false);
            _userRepository.Add(_testUser);
            _testArticle = new Article("Test Article", "Test Content", 1, _testUser);
            _articleRepository.Add(_testArticle);

            anArticle = new Article() { Id = 1, Name = "Test Article", Body = "Test body", Private = false, DateModified = DateTime.Now, Template = 1, Image = null, DateDeleted = null };
            articles = new List<Article>() { anArticle };
            anotherName = "otherName";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }


        [TestMethod]
        public void AddArticleOk()
        {
            Article _testArticle2 = new Article("Test Article", "Test Content", 1, _testUser);
            _articleRepository.Add(_testArticle2);
            Article articleInDb = _dbContext.Articles.Where(a => a.Id == _testArticle.Id).AsNoTracking().FirstOrDefault();

            Assert.IsNotNull(articleInDb);
            Assert.AreEqual(anArticle.Id, articleInDb.Id);
        }

        [TestMethod]
        public void AddArticleFail()
        {
            Assert.ThrowsException<AlreadyExistsDbException>(() => _articleRepository.Add(anArticle));
        }

        [TestMethod]
        public void GetArticleById_ShouldReturnCorrectArticle()
        {
            Article retrievedArticle = _articleRepository.Get(a => a.Id == _testArticle.Id);

            Assert.AreEqual(_testArticle, retrievedArticle);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void GetArticleById_ShouldThrowNotFoundDbException()
        {
            Article retrievedArticle = _articleRepository.Get(a => a.Id == -1);
        }

        [TestMethod]
        public void Exists_ShouldReturnTrue_WhenArticleExists()
        {
            var article = new Article("Article 1", "Body of article 1", 1, _testUser);
            _dbContext.Set<Article>().Add(article);
            _dbContext.SaveChanges();

            bool result = _articleRepository.Exists(a => a.Id == article.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Exists_ShouldReturnFalse_WhenArticleDoesNotExist()
        {
            var article = new Article("Article 1", "Body of article 1", 1, _testUser);
            _dbContext.Set<Article>().Add(article);
            _dbContext.SaveChanges();

            bool result = _articleRepository.Exists(a => a.Id == article.Id + 1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_ShouldReturnArticle_WhenArticleExists()
        {
            var article1 = new Article("Article 1", "Body of article 1", 1, _testUser);
            var article2 = new Article("Article 2", "Body of article 2", 1, _testUser);
            _dbContext.Set<Article>().Add(article1);
            _dbContext.Set<Article>().Add(article2);
            _dbContext.SaveChanges();

            var result = _articleRepository.Get(a => a.Id == article1.Id);

            Assert.AreEqual(article1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void Get_ShouldThrowNotFoundDbException_WhenArticleDoesNotExist()
        {
            var article = new Article("Article 1", "Body of article 1", 1, _testUser);
            _dbContext.Set<Article>().Add(article);
            _dbContext.SaveChanges();

            var result = _articleRepository.Get(a => a.Id == article.Id + 1);

        }

        [TestMethod]
        public void Update_ShouldThrowNotFoundDbException_WhenArticleDoesNotExist()
        {
            ArticleRepository articleRepository = new ArticleRepository(_dbContext);

            Article articleToUpdate = new Article()
            {
                Id = 9999,
                Name = "Article to update",
                Body = "Body of article to update",
                Private = false,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                User = _testUser,
                UserId = 1,
                Comments = new List<Comment>(),
                Template = 1,
                Image = "article.jpg"
            };

            Assert.ThrowsException<NotFoundDbException>(() => articleRepository.Update(articleToUpdate));
        }

        [TestMethod]
        public void Update_ShouldUpdateArticle_WhenArticleExists()
        {
            ArticleRepository articleRepository = new ArticleRepository(_dbContext);

            Article articleToUpdate = new Article()
            {
                Id = 1,
                Name = "Updated article",
                Body = "Updated body of article",
                Private = true,
                DateCreated = DateTime.Now.AddDays(-10),
                DateModified = DateTime.Now,
                User = _testUser,
                UserId = 1,
                Comments = new List<Comment>(),
                Template = 2,
                Image = "updated.jpg"
            };

            articleRepository.Update(articleToUpdate);

            Article updatedArticle = articleRepository.Get(a => a.Id == 1);
            Assert.AreEqual(articleToUpdate.Id, updatedArticle.Id);
            Assert.AreEqual(articleToUpdate.Name, updatedArticle.Name);
            Assert.AreEqual(articleToUpdate.Body, updatedArticle.Body);
            Assert.AreEqual(articleToUpdate.Private, updatedArticle.Private);
            Assert.AreEqual(articleToUpdate.DateCreated, updatedArticle.DateCreated);
            Assert.AreEqual(articleToUpdate.DateModified, updatedArticle.DateModified);
            Assert.AreEqual(articleToUpdate.UserId, updatedArticle.UserId);
            Assert.AreEqual(articleToUpdate.Template, updatedArticle.Template);
            Assert.AreEqual(articleToUpdate.Image, updatedArticle.Image);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllArticles_WhenFuncIsNull()
        {
            var user = new User { Username = "user1", Email = "user1@test.com" };
            var article1 = new Article { Name = "Article 1", Body = "Body 1", User = _testUser };
            var article2 = new Article { Name = "Article 2", Body = "Body 2", User = _testUser };
            var article3 = new Article { Name = "Article 3", Body = "Body 3", User = _testUser };
            _dbContext.Set<Article>().AddRange(article1, article2, article3);
            _dbContext.SaveChanges();

            var result = _articleRepository.GetAll(a => true);

            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.Contains(article1));
            Assert.IsTrue(result.Contains(article2));
            Assert.IsTrue(result.Contains(article3));
        }

        [TestMethod]
        public void GetAll_ShouldReturnFilteredArticles_WhenFuncIsProvided()
        {
            User user1 = new User("username", "password", "email@.com", "name", "last_name", false, false);
            User user2 = new User("username2", "password", "email@.com", "name", "last_name", false, false);
            var article1 = new Article { Name = "Article 1", Body = "Body 1", User = user1 };
            var article2 = new Article { Name = "Article 2", Body = "Body 2", User = user1 };
            var article3 = new Article { Name = "Article 3", Body = "Body 3", User = user2 };
            _dbContext.Set<Article>().AddRange(article1, article2, article3);
            _dbContext.SaveChanges();

            var result = _articleRepository.GetAll(a => a.User == user1);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(article1));
            Assert.IsTrue(result.Contains(article2));
        }

        [TestMethod]
        public void GetAll_ShouldThrowNotFoundDbException_WhenNoArticleExists()
        {
            Assert.ThrowsException<NotFoundDbException>(() => _articleRepository.GetAll(a => a.Name == "Nonexistent"));
        }
    }
}