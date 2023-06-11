using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.IBusinessLogic.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class OffensiveWordsValidator : IOffensiveWordsValidator
    {
        private readonly IOffensiveWordRepository _offensiveWordRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private List<string> offensiveWords;

        public OffensiveWordsValidator(IOffensiveWordRepository offensiveWordRepository, IArticleRepository articleRepository, ICommentRepository commentRepository)
        {
            this._offensiveWordRepository = offensiveWordRepository;
            offensiveWords = offensiveWordRepository.GetAll(w => true).Select(w => w.Word).ToList();
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
        }

        private List<string> FindOffensiveWords(string content)
        {
            content = content.ToLower();
            List<string> foundOffensiveWords = new List<string>();

            foreach (string word in offensiveWords)
            {
                if (content.Contains(word.ToLower()))
                {
                    foundOffensiveWords.Add(word);
                }
            }

            return foundOffensiveWords;
        }

        public void NotifyAdminsAndModerators(string content, List<string> offensiveWords)
        {
            throw new NotImplementedException();
        }

        public void AddOffensiveWord(User loggedUser, string word)
        {
            if (!loggedUser.Admin && !loggedUser.Moderador)
                throw new UnauthorizedAccessException("No tiene permisos para realizar esta acción.");

            OffensiveWord offensiveWord = new OffensiveWord { Word = word };
            _offensiveWordRepository.Add(offensiveWord);
            offensiveWords.Add(word);
        }

        public void RemoveOffensiveWord(User loggedUser, string word)
        {
            if (!loggedUser.Admin && !loggedUser.Moderador)
                throw new UnauthorizedAccessException("No tiene permisos para realizar esta acción.");

            OffensiveWord offensiveWord = _offensiveWordRepository.Get(w => w.Word == word);
            if (offensiveWord != null)
            {
                _offensiveWordRepository.Remove(offensiveWord);
                offensiveWords.Remove(word);
            }
        }

        public List<string> reviewArticle(Article article)
        {
            string content = (article.Name).Concat(article.Body).ToString();
            return FindOffensiveWords(content);
        }

        public List<string> reviewComment(Comment comment)
        {
            return FindOffensiveWords(comment.Body);
        }
    }

}

