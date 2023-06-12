using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IOffensiveWordsValidator
    {
        List<string> reviewArticle(Article article);
        List<string> reviewComment(Comment comment);
        void NotifyAdminsAndModerators(string content, List<string> offensiveWords);
        void AddOffensiveWord(User loggedUser, string word);
        void RemoveOffensiveWord(User loggedUser, string word);
        ICollection<Article> GetArticlesToReview(User loggedUser);
        ICollection<Comment> GetCommentsToReview(User loggedUser);
        void UnflagReviewContentForUser(User loggedUser, User userToUnflag);
    }

}

