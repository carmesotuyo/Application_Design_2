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

        public User DeleteUser(User loggedUser, int UserId)
        {
            if (authorizedUser(loggedUser, UserId))
            {
                if (_userRepository.Exists(m => m.Id == UserId))
                {
                    User user = _userRepository.Get(m => m.DateDeleted == null && m.Id == UserId);
                    user.DateDeleted = DateTime.Now;
                    foreach (Article article in user.Articles)
                    {
                        _articleLogic.DeleteArticle(article.Id, user);
                    }
                    _userRepository.Update(user);
                    return user;
                }
                else
                {
                    throw new NotFoundDbException("No existe un usuario con ese id.");
                }
            }
            else 
            {
                throw new UnauthorizedAccessException("No está autorizado para realizar esta acción.");
            }
        }

        public ICollection<User> GetUsersRanking(User loggedUser, DateTime dateFrom, DateTime dateTo, int? top)
        {
            if(loggedUser != null && loggedUser.Admin)
            {
                return _userRepository.GetAll(m => m.DateDeleted == null)
                                                .Select(m => new
                                                {
                                                    User = m,
                                                    Points = m.Articles.Count(a => a.DateCreated >= dateFrom && a.DateCreated <= dateTo)
                                                              + m.Comments.Count(c => c.DateCreated >= dateFrom && c.DateCreated <= dateTo)
                                                })
                                                .Where(m => m.Points > 0)
                                                .OrderByDescending(m => m.Points)
                                                .ThenBy(m => m.User.Id)
                                                .Take(top ?? 10)
                                                .Select(m => m.User)
                                                .ToList();
            } else
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

        public User? UpdateUser(User loggedUser, User anUser)
        {
            if (authorizedUser(loggedUser, anUser.Id))
            {
                if (_userRepository.Exists(m => m.Id == anUser.Id))
                {
                    User user = _userRepository.Get(m => m.DateDeleted == null && m.Id == anUser.Id);
                    user = anUser;
                    _userRepository.Update(user);
                    return user;
                }
                else
                {
                    throw new NotFoundDbException("No existe un usuario con ese id.");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("No está autorizado para realizar esta acción.");
            }
        }

        private bool authorizedUser(User loggedUser, int userId) 
        { 
            if (loggedUser != null && (loggedUser.Admin || loggedUser.Id == userId))
            {
                return true;
            } else
            {
               return false;
            }
        }

    }
}
