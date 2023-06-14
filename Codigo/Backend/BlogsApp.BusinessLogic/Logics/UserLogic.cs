using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.Domain.Exceptions;
using System.Data;
using BlogsApp.DataAccess.Interfaces.Exceptions;

namespace BlogsApp.BusinessLogic.Logics
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly IArticleLogic _articleLogic;

        public UserLogic(IUserRepository userRepository, IArticleLogic articleLogic)
        {
            _userRepository = userRepository;
            _articleLogic = articleLogic;
        }

        public User CreateUser(User user)
        {
            IsUserValid(user);
            _userRepository.Add(user!);
            return user;
        }

        public User GetUserById(int userId)
        {
            return _userRepository.Get(m => m.DateDeleted == null && m.Id == userId);
        }

        public IEnumerable<User> GetUsers(User loggedUser)
        {
            if ((loggedUser != null) && (IsAdmin(loggedUser)))
            {
                return _userRepository.GetAll(m => m.DateDeleted == null)
                                 .OrderByDescending(m => m.Name);
            }
            else
            {
                throw new UnauthorizedAccessException("No está autorizado para realizar esta acción.");
            }
        }

        public bool IsAdmin(User loggedUser)
        {
            return loggedUser.Admin;
        }

        public bool IsModerator(User loggedUser)
        {
            return loggedUser.Moderador;
        }

        public bool IsBlogger(User loggedUser)
        {
            return loggedUser.Blogger;
        }

        public User DeleteUser(User loggedUser, int UserId)
        {
            validateAuthorizedUser(loggedUser, UserId);
            validateUserExists(UserId);
            User user = _userRepository.Get(m => m.DateDeleted == null && m.Id == UserId);
            user.DateDeleted = DateTime.Now;
            if (user.Articles != null)
            {
                foreach (Article article in user.Articles)
                {
                    _articleLogic.DeleteArticle(article.Id, user);
                }
            }
            
            _userRepository.Update(user);
            return user;
        }

        public ICollection<User> GetUsersRanking(User loggedUser, DateTime dateFrom, DateTime dateTo, int? top, bool? offensiveContent = false)
        {
            if(loggedUser != null && loggedUser.Admin)
            {
                Func<Content, bool> filterContent = c => c.DateCreated >= dateFrom && c.DateCreated <= dateTo && c.State != Domain.Enums.ContentState.InReview;
                if (offensiveContent != null && offensiveContent == true) filterContent = c => c.HadOffensiveWords && c.DateCreated >= dateFrom && c.DateCreated <= dateTo && c.State != Domain.Enums.ContentState.InReview;

                return _userRepository.GetAll(u => u.DateDeleted == null)
                                            .Select(u => new 
                                            {
                                                User = u,
                                                Points = _userRepository.GetUserContentCount((m => m.DateDeleted == null && m.Id == u.Id), filterContent)
                                            })
                                            .Where(m => m.Points > 0)
                                            .OrderByDescending(m => m.Points)
                                            .ThenBy(m => m.User.Id)
                                            .Take(top ?? 10)
                                            .Select(m => m.User)
                                            .ToList();
            }
            else
            {
                throw new UnauthorizedAccessException("No está autorizado para realizar esta acción.");
            }
        }

        public bool IsUserValid(User? user)
        {
            if (user == null)
            {
                throw new BadInputException("Usuario inválido");
            }
            return true;
        }
        public User? UpdateUser(User loggedUser, User userWithDataToUpdate)
        {
            User userFromDB = GetUserById(userWithDataToUpdate.Id);
            if ((!IsAdmin(loggedUser)) && (loggedUser.Id == userWithDataToUpdate.Id))
            {
                if ((userFromDB.Admin != userWithDataToUpdate.Admin) || (userFromDB.Moderador != userWithDataToUpdate.Moderador))
                {
                    throw new UnauthorizedAccessException(loggedUser.Name + " No te puedes asignar nuevos roles");
                }
                else
                {
                    _userRepository.Update(userWithDataToUpdate);
                    return userWithDataToUpdate;
                }
            }
            else
            {
                if (IsAdmin(loggedUser))
                {
                    _userRepository.Update(userWithDataToUpdate);
                    return userWithDataToUpdate;
                }
                else { throw new UnauthorizedAccessException("No está autorizado para modificar los datos de otro usuario."); }
            }
        }


        private void validateAuthorizedUser(User loggedUser, int userWithDataToUpdateID)
        {
            if (loggedUser == null || (!loggedUser.Admin && loggedUser.Id != userWithDataToUpdateID))
                throw new UnauthorizedAccessException("No está autorizado para realizar esta acción.");
        }

        private void validateUserExists(int userId)
        {
            if (!_userRepository.Exists(m => m.Id == userId))
                throw new NotFoundDbException("No existe un usuario con ese id.");
        }
    }
}
