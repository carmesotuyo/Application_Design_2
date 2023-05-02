using BlogsApp.DataAccess.Interfaces.Exceptions;
using BlogsApp.Domain.Entities;
using BlogsApp.Domain.Exceptions;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class SessionLogic : ISessionLogic
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public SessionLogic(ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
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
            User user = correctCredentials(username, password);
            Guid token = Guid.NewGuid();
            Session session = new Session(user, token);
            _sessionRepository.Add(session);
            return token;
        }

        private User correctCredentials(string username, string password)
        {
            if (_userRepository.Exists(m => m.Username == username))
            {
                User user = _userRepository.Get(m => m.DateDeleted == null && m.Username == username);
                if (user.Password == password)
                {
                    return user;
                } else
                {
                    throw new BadInputException("Usuario o contraseña incorrectos");
                }
            }
            else
            {
                throw new NotFoundDbException("No existe el usuario");
            }
        }

        public void Logout(int sessionId, User loggedUser)
        {
            throw new NotImplementedException();
        }
    }
}
