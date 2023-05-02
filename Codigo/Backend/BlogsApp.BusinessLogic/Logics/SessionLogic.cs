using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class SessionLogic : ISessionLogic
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionLogic(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public IEnumerable<Comment> GetCommentsWhileLoggedOut(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUserFromToken(Guid aToken)
        {
            throw new NotImplementedException();
        }

        public bool IsValidToken(string token)
        {
            throw new NotImplementedException();
        }

        public Guid Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout(int sessionId, User loggedUser)
        {
            throw new NotImplementedException();
        }
    }
}
