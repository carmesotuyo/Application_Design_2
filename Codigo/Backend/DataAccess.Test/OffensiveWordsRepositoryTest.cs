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
	public class OffensiveWordsRepositoryTest
    {
        private OffensiveWord word;
        private OffensiveWord existingWord;

        private Context _dbContext;
        private OffensiveWordRepository _offensiveWordsRepository;

        [TestInitialize]
        public void TestInit()
        {
            var options = new DbContextOptionsBuilder<Context>()
                        .UseInMemoryDatabase(databaseName: "OffensiveWordsDb")
                        .Options;
            _dbContext = new Context(options);
            _offensiveWordsRepository = new OffensiveWordRepository(_dbContext);
            word = new OffensiveWord() { Id = 1, Word = "offensive" };
            existingWord = new OffensiveWord() { Id = 2, Word = "really offensive" };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void AddWordOk()
        {
            _offensiveWordsRepository.Add(word);
            OffensiveWord wordInDb = _dbContext.OffensiveWords.Where(a => a.Id == word.Id).AsNoTracking().FirstOrDefault();

            Assert.IsNotNull(wordInDb);
            Assert.AreEqual(word.Id, wordInDb.Id);
            Assert.AreEqual(word.Word, wordInDb.Word);
        }

        [TestMethod]
        public void AddDuplicateWordFail()
        {
            _offensiveWordsRepository.Add(existingWord);

            Assert.ThrowsException<AlreadyExistsDbException>(() => _offensiveWordsRepository.Add(existingWord));
        }


        [TestMethod]
        public void Exists_ShouldReturnTrue_WhenWordExists()
        {
            _dbContext.Set<OffensiveWord>().Add(existingWord);
            _dbContext.SaveChanges();

            bool result = _offensiveWordsRepository.Exists(a => a.Word == existingWord.Word);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Exists_ShouldReturnFalse_WhenWordDoesNotExist()
        {
            // Empty DB 

            bool result = _offensiveWordsRepository.Exists(a => a.Word == word.Word);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Get_ShouldReturnWord_WhenWordExists()
        {
            _dbContext.Set<OffensiveWord>().Add(existingWord);
            _dbContext.SaveChanges();

            var result = _offensiveWordsRepository.Get(a => a.Word == existingWord.Word);

            Assert.AreEqual(existingWord, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void Get_ShouldThrowNotFoundDbException_WhenWordDoesNotExist()
        {
            // Empty DB
            var result = _offensiveWordsRepository.Get(a => a.Word == word.Word);
        }


        [TestMethod]
        public void GetAll_ShouldReturnAllWords_WhenFuncIsNull()
        {
            _dbContext.Set<OffensiveWord>().AddRange(word, existingWord);
            _dbContext.SaveChanges();

            var result = _offensiveWordsRepository.GetAll(a => true);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(word));
            Assert.IsTrue(result.Contains(existingWord));
        }

        [TestMethod]
        public void GetAll_ShouldReturnFilteredWords_WhenFuncIsProvided()
        {
            _dbContext.Set<OffensiveWord>().AddRange(word, existingWord);
            _dbContext.SaveChanges();

            var result = _offensiveWordsRepository.GetAll(a => a.Word == word.Word);

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(word));
            Assert.IsFalse(result.Contains(existingWord));
        }

        [TestMethod]
        public void RemoveOk()
        {
            _dbContext.Set<OffensiveWord>().Add(existingWord);
            _dbContext.SaveChanges();
            Assert.IsTrue(_dbContext.Set<OffensiveWord>().Contains(existingWord));

            _offensiveWordsRepository.Remove(existingWord);
            Assert.IsFalse(_dbContext.Set<OffensiveWord>().Contains(existingWord));
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDbException))]
        public void RemoveNonExistingWord()
        {
            // Empty DB
            _offensiveWordsRepository.Remove(word);
        }
    }
}

