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
        private readonly ICommentLogic _commentLogic;

        public SessionLogic(ISessionRepository sessionRepository, IUserRepository userRepository, ICommentLogic commentLogic)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _commentLogic = commentLogic;
        }

        public IEnumerable<Comment> GetCommentsWhileLoggedOut(User loggedUser)
        {
            DateTime? lastLogout = GetLastLogoutDateTime(loggedUser.Id);
            if (lastLogout == null) return new List<Comment>();
            return _commentLogic.GetCommentsSince(loggedUser, lastLogout);
        }

        private DateTime? GetLastLogoutDateTime(int userId)
        {
            return _sessionRepository.GetAll(s => s.User.Id == userId && s.DateTimeLogout != null)
                                     .OrderByDescending(s => s.DateTimeLogout)
                                     .FirstOrDefault()?.DateTimeLogout;
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
            Session logOutSession = _sessionRepository.Get(IsValidSession(sessionId, loggedUser));
            if (logOutSession != null)
            {
                logOutSession.DateTimeLogout = DateTime.Now;
                _sessionRepository.Update(logOutSession);
            }
            else
            {
                throw new UnauthorizedAccessException("Sesión inválida, no se deslogueó");
            }
        }

        private Func<Session, bool> IsValidSession(int sessionId, User loggedUser)
        {
            return s => s.User == loggedUser && s.DateTimeLogout == null && s.Id == sessionId;
        }

        public bool IsValidToken(string token)
        {
            Guid guidToken;
            Guid.TryParse(token, out guidToken);
            return token != null && _sessionRepository.Exists(s => s.Token == guidToken && s.DateTimeLogout == null);
        }

        public User GetUserFromToken(Guid aToken)
        {
            bool exists = _sessionRepository.Exists(s => s.Token == aToken);
            if (!exists) throw new NotFoundDbException("Token inválido");
            return _sessionRepository.Get(s => s.Token == aToken).User;
        }
    }
}