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

        private void UnnotifyAdminsAndModerators()
        {
            ICollection<User> adminsAndModerators = _userRepository.GetAll(u => u.DateDeleted == null && (u.Admin || u.Moderador));
            foreach (User user in adminsAndModerators)
            {
                user.HasContentToReview = false;
                _userRepository.Update(user);
            }
        }

        private OffensiveWord GetOffensiveWord(string word)
        {
            return _offensiveWordRepository.Get(w => w.Word == word);
        }


        public ICollection<OffensiveWord> mapToOffensiveWordsType(ICollection<string> offensiveWords)
        {
            ICollection<OffensiveWord> result = new List<OffensiveWord>();
            foreach (string word in offensiveWords)
            {
                result.Add(GetOffensiveWord(word));
            }
            return result;
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
            ICollection<Article> articles = new List<Article>();
            try
            {
                articles = _articleRepository.GetAll(a => a.DateDeleted == null && a.State == Domain.Enums.ContentState.InReview);
            }
            catch (NotFoundDbException ex) { }
            return articles;
        }

        public ICollection<Comment> GetCommentsToReview(User loggedUser)
        {
            validateAuthorizedUser(loggedUser);
            ICollection<Comment> comments = new List<Comment>();
            try
            {
                return _commentRepository.GetAll(a => a.DateDeleted == null && a.State == Domain.Enums.ContentState.InReview);
            }
            catch (NotFoundDbException ex)
            {
                throw new NotFoundDbException("No se encontró contenido para revisar");
            }
        }

        private int CountContentToReview()
        {
            int contentCount = 0;
            try
            {
                ICollection<Article> articles = _articleRepository.GetAll(a => a.DateDeleted == null && a.State == Domain.Enums.ContentState.InReview);
                contentCount += articles.Count();

            }
            catch (NotFoundDbException ex) { }

            try
            {
                ICollection<Comment> comments = _commentRepository.GetAll(a => a.DateDeleted == null && a.State == Domain.Enums.ContentState.InReview);
                contentCount += comments.Count();

            }
            catch (NotFoundDbException ex) { }

            if (contentCount == 0) UnnotifyAdminsAndModerators();
            return contentCount;
        }

        private void validateAuthorizedUser(User loggedUser)
        {
            if (!loggedUser.Admin && !loggedUser.Moderador)
                throw new UnauthorizedAccessException("No tiene permiso para revisar el contenido");
        }

        public bool checkUserHasContentToReview(User loggedUser)
        {
            return loggedUser.HasContentToReview && CountContentToReview() > 0;
        }

        public void UnflagReviewContentForUser(User loggedUser, User userToUnflag)
        {
            if (loggedUser.Id != userToUnflag.Id)
                throw new UnauthorizedAccessException("No tiene permiso para realizar esta acción");

            userToUnflag.HasContentToReview = false;
            _userRepository.Update(userToUnflag);
        }

        public ICollection<string> GetOffensiveWords(User loggedUser)
        {
            return offensiveWords;
        }
    }

}

