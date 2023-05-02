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
            // Arrange
            var article = new Article("Article 1", "Body of article 1", 1, _testUser);
            _dbContext.Set<Article>().Add(article);
            _dbContext.SaveChanges();

            // Act
            bool result = _articleRepository.Exists(a => a.Id == article.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Exists_ShouldReturnFalse_WhenArticleDoesNotExist()
        {
            // Arrange
            var article = new Article("Article 1", "Body of article 1", 1, _testUser);
            _dbContext.Set<Article>().Add(article);
            _dbContext.SaveChanges();

            // Act
            bool result = _articleRepository.Exists(a => a.Id == article.Id + 1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_ShouldReturnArticle_WhenArticleExists()
        {
            // Arrange
            var article1 = new Article("Article 1", "Body of article 1", 1, _testUser);
            var article2 = new Article("Article 2", "Body of article 2", 1, _testUser);
            _dbContext.Set<Article>().Add(article1);
            _dbContext.Set<Article>().Add(article2);
            _dbContext.SaveChanges();

            // Act
            var result = _articleRepository.Get(a => a.Id == article1.Id);

            // Assert
            Assert.AreEqual(article1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void Get_ShouldThrowNotFoundDbException_WhenArticleDoesNotExist()
        {
            // Arrange
            var article = new Article("Article 1", "Body of article 1", 1, _testUser);
            _dbContext.Set<Article>().Add(article);
            _dbContext.SaveChanges();

            // Act
            var result = _articleRepository.Get(a => a.Id == article.Id + 1);

        }
















        //[TestMethod]
        //public void GetArticleOk()
        //{
        //    getById = GetArticleById(anArticle.Id);

        //    Article articleInDb = articleRepository.Get(getById);

        //    Assert.IsNotNull(articleInDb);
        //    Assert.AreEqual(anArticle.Id, articleInDb.Id);
        //}

        //[TestMethod]
        //public void GetArticleFail()
        //{
        //    var context = CreateContext();
        //    IArticleRepository articleRepository = new ArticleRepository(context);
        //    getById = GetArticleById(anArticle.Id);

        //    Assert.ThrowsException<NotFoundDbException>(() => articleRepository.Get(getById));
        //}

        //[TestMethod]
        //public void GetAllArticlesOk()
        //{
        //    IArticleRepository articleRepository = CreateRepositoryWithArticle();

        //    ICollection<Article> articlesInDb = articleRepository.GetAll(a => true);

        //    Assert.IsNotNull(articlesInDb);
        //    Assert.IsTrue(articlesInDb.SequenceEqual(articles));
        //}

        //[TestMethod]
        //public void GetAllArticlesFail()
        //{
        //    var context = CreateContext();
        //    IArticleRepository articleRepository = new ArticleRepository(context);

        //    Assert.ThrowsException<NotFoundDbException>(() => articleRepository.GetAll(m => true));
        //}

        //[TestMethod]
        //public void ExistsArticleTrue()
        //{
        //    var context = CreateContext();
        //    IArticleRepository articleRepository = CreateRepositoryWithArticle();
        //    getById = GetArticleById(anArticle.Id);

        //    articleRepository.Exists(getById);
        //    bool existsArticle = context.Articles.Any(a => a.Id == anArticle.Id);

        //    Assert.IsTrue(existsArticle);
        //}

        //[TestMethod]
        //public void UpdateArticleNotFound()
        //{
        //    var context = CreateContext();
        //    IArticleRepository articleRepository = new ArticleRepository(context);
        //    anArticle.Name = anotherName;

        //    Assert.ThrowsException<NotFoundDbException>(() => articleRepository.Update(anArticle));
        //}

        //[TestMethod]
        //public void UpdateArticleOk()
        //{
        //    var context = CreateContext();
        //    IArticleRepository articleRepository = CreateRepositoryWithArticle();
        //    anArticle.Name = anotherName;

        //    articleRepository.Update(anArticle);
        //    Article updatedArticle = context.Articles.Where(m => m.Id == anArticle.Id).AsNoTracking().FirstOrDefault();

        //    Assert.IsNotNull(updatedArticle);
        //    Assert.AreEqual(anotherName, updatedArticle.Name);
        //}







        //======================================================================

        //private Context GetMemoryContext(string nameBd)
        //{
        //    var builder = new DbContextOptionsBuilder<Context>();
        //    return new Context(GetMemoryConfig(builder, nameBd));
        //}
        //private DbContextOptions GetMemoryConfig(DbContextOptionsBuilder builder, string nameBd)
        //{
        //    builder.UseInMemoryDatabase(nameBd);
        //    return builder.Options;
        //}
        //private IArticleRepository CreateRepositoryWithArticle()
        //{
        //    var context = CreateContext();
        //    context.Articles.Add(anArticle);
        //    context.SaveChanges();

        //    IArticleRepository repository = new ArticleRepository(context);
        //    return repository;
        //}
        //private Context CreateContext()
        //{
        //    Context context = GetMemoryContext("MemoryTestArticleDB");
        //    context.Database.EnsureDeleted();
        //    context.Database.EnsureCreated();
        //    return context;
        //}
        //private Func<Article, bool> GetArticleById(int Id)
        //{
        //    return a => a.Id == Id;
        //}
    }
}