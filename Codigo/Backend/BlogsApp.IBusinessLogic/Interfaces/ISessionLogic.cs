using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface ISessionLogic
    {
        bool IsValidToken(string token);
        Guid Login(string username, string password);
        User GetUserFromToken(Guid aToken);
        void Logout(User loggedUser);
        IEnumerable<Comment> GetCommentsWhileLoggedOut(int userId);
    }
}
