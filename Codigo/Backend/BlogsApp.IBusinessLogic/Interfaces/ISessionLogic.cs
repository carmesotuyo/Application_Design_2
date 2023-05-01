using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface ISessionLogic
    {
        bool IsValidToken(string token);
        Guid Login(string username, string password);
        void Logout(User loggedUser);
    }
}
