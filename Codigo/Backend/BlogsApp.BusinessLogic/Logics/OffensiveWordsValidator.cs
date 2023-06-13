using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.DataAccess.Interfaces.Exceptions;

namespace BlogsApp.BusinessLogic.Logics
{
    public class OffensiveWordsValidator : IOffensiveWordsValidator
    {
        private readonly IOffensiveWordRepository _offensiveWordRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private List<string> offensiveWords;

        public OffensiveWordsValidator(IOffensiveWordRepository offensiveWordRepository, IArticleRepository articleRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            this._offensiveWordRepository = offensiveWordRepository;
            offensiveWords = offensiveWordRepository.GetAll(w => true).Select(w => w.Word).ToList();
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
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

        public void NotifyAdminsAndModerators()
        {
            ICollection<User> adminsAndModerators = _userRepository.GetAll(u => u.DateDeleted == null && (u.Admin || u.Moderador));
            foreach(User user in adminsAndModerators)
            {
                user.HasContentToReview = true;
                _userRepository.Update(user);
            }
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
            string content = article.Name + article.Body;
            return FindOffensiveWords(content);
        }

        public List<string> reviewComment(Comment comment)
        {
            return FindOffensiveWords(comment.Body);
        }

        public ICollection<Article> GetArticlesToReview(User loggedUser)
        {
            validateAuthorizedUser(loggedUser);
            checkUserHasContentToReview(loggedUser);
            return _articleRepository.GetAll(a => a.DateDeleted == null && a.State == Domain.Enums.ContentState.InReview);
        }

        public ICollection<Comment> GetCommentsToReview(User loggedUser)
        {
            validateAuthorizedUser(loggedUser);
            checkUserHasContentToReview(loggedUser);
            return _commentRepository.GetAll(a => a.DateDeleted == null && a.State == Domain.Enums.ContentState.InReview);
        }

        private void validateAuthorizedUser(User loggedUser)
        {
            if (!loggedUser.Admin && !loggedUser.Moderador)
                throw new UnauthorizedAccessException("No tiene permiso para revisar el contenido");
        }

        private void checkUserHasContentToReview(User loggedUser)
        {
            if (!loggedUser.HasContentToReview)
                throw new NotFoundDbException("No tienes contenido para revisar");
        }

        public void UnflagReviewContentForUser(User loggedUser, User userToUnflag)
        {
            if (loggedUser.Id != userToUnflag.Id)
                throw new UnauthorizedAccessException("No tiene permiso para realizar esta acción");

            userToUnflag.HasContentToReview = false;
            _userRepository.Update(userToUnflag);
        }
    }

}

