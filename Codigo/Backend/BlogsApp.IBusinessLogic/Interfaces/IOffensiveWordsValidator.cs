using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IOffensiveWordsValidator
    {
        List<string> reviewArticle(Article article);
        List<string> reviewComment(Comment comment);
        void NotifyAdminsAndModerators();
        void AddOffensiveWord(User loggedUser, string word);
        void RemoveOffensiveWord(User loggedUser, string word);
        ICollection<Article> GetArticlesToReview(User loggedUser);
        ICollection<Comment> GetCommentsToReview(User loggedUser);
        void UnflagReviewContentForUser(User loggedUser, User userToUnflag);
        ICollection<OffensiveWord> mapToOffensiveWordsType(ICollection<string> offensiveWords);
    }

    public abstract class OffensiveWordsValidatorUtils
    {
        public static ICollection<string> mapToStrings(ICollection<OffensiveWord> offensiveWords)
        {
            ICollection<string> result = new List<string>();
            foreach (OffensiveWord word in offensiveWords)
            {
                result.Add(word.Word);
            }
            return result;
        }
    }

}

