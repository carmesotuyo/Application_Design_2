using System.Collections.Generic;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogsApp.Domain;
using System.Linq.Expressions;
using BlogsApp.DataAccess.Migrations;
using BlogsApp.DataAccess.Repositories;

namespace BusinessLogic.Test
{
	[TestClass]
	public class OffensiveWordsValidatorTest
	{
		private Mock<IOffensiveWordRepository> offensiveWordsRepo;
        private Mock<IArticleRepository> articleRepository;
        private Mock<ICommentRepository> commentRepository;
        private IOffensiveWordsValidator offensiveWordsValidator;
        private ICollection<OffensiveWord> offensiveWords;
        private readonly User moderator = new User() { Moderador = true };
        private readonly User commonUser = new User() { Moderador = false, Admin = false };
        private OffensiveWord word;
        private OffensiveWord word2;
        private Article article;
        private Comment comment;


        [TestInitialize]
        public void TestInitialize()
        {
            word = new OffensiveWord() { Id = 1, Word = "offensive" };
            word2 = new OffensiveWord() { Id = 2, Word = "offensive2" };
            offensiveWords = new List<OffensiveWord>() { word, word2 };

            offensiveWordsRepo = new Mock<IOffensiveWordRepository>(MockBehavior.Strict);
            offensiveWordsRepo.Setup(x => x.GetAll(It.IsAny<Func<OffensiveWord, bool>>())).Returns(offensiveWords);

            articleRepository = new Mock<IArticleRepository>(MockBehavior.Strict);
            commentRepository = new Mock<ICommentRepository>(MockBehavior.Strict);

            offensiveWordsValidator = new OffensiveWordsValidator(offensiveWordsRepo.Object, articleRepository.Object, commentRepository.Object);
            article = new Article() { Name = "offensive", Body = "something offensive" };
            comment = new Comment() { Body = "this is offensive" };
        }

        [TestMethod]
        public void NotifyAdminsAndModeratorsTest()
        {
            // COMPLETE
        }

        [TestMethod]
        public void AddOffensiveWordOk()
        {
            offensiveWordsRepo.Setup(x => x.Add(It.IsAny<OffensiveWord>())).Returns(word);
            offensiveWordsValidator.AddOffensiveWord(moderator, word.Word);
            offensiveWordsRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AddOffensiveWordWithoutPermission()
        {
            offensiveWordsRepo.Setup(x => x.Add(It.IsAny<OffensiveWord>())).Returns(word);
            offensiveWordsValidator.AddOffensiveWord(commonUser, word.Word);
        }

        [TestMethod]
        public void RemoveOffensiveWordOk()
        {
            offensiveWordsRepo.Setup(x => x.Get(It.IsAny<Func<OffensiveWord, bool>>())).Returns(word);
            offensiveWordsRepo.Setup(x => x.Remove(It.IsAny<OffensiveWord>()));
            offensiveWordsValidator.RemoveOffensiveWord(moderator, word.Word);
            offensiveWordsRepo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void RemoveOffensiveWordWithoutPermission()
        {
            offensiveWordsRepo.Setup(x => x.Get(It.IsAny<Func<OffensiveWord, bool>>())).Returns(word);
            offensiveWordsRepo.Setup(x => x.Remove(It.IsAny<OffensiveWord>()));
            offensiveWordsValidator.RemoveOffensiveWord(commonUser, word.Word);
        }

        [TestMethod]
        public void ReviewArticleWithOneOffensiveWord()
        {
            List<string> result = offensiveWordsValidator.reviewArticle(article);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void ReviewArticleWithMultipleOffensiveWords()
        {
            article.Body = "offensive offensive2";

            List<string> result = offensiveWordsValidator.reviewArticle(article);
            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public void ReviewArticleWithoutOffensiveWords()
        {
            article.Name = "good";
            article.Body = "nice text";

            List<string> result = offensiveWordsValidator.reviewArticle(article);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void ReviewCommentWithOneOffensiveWord()
        {
            List<string> result = offensiveWordsValidator.reviewComment(comment);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void ReviewCommentWithMultipleOffensiveWords()
        {
            comment.Body = "offensive offensive2";

            List<string> result = offensiveWordsValidator.reviewComment(comment);
            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public void ReviewCommentWithoutOffensiveWords()
        {
            comment.Body = "nice text";

            List<string> result = offensiveWordsValidator.reviewComment(comment);
            Assert.IsTrue(result.Count() == 0);
        }
    }
}

