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

        //public User DeleteUser1(int userId)
        //{
        //    User user = _userRepository.Get(UserById(userId));
        //    user.DateDeleted = DateTime.Now;
        //    _userRepository.Update(user);
        //    return user;
        //}

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
                    throw new ExistenceException("No existe un usuario con ese id.");
                }
            }
            else 
            {
                throw new UnauthorizedAccessException("No está autorizado para realizar esta acción.");
            }
        }

       
        public ICollection<User> GetUsersRanking(User loggedUser, DateTime dateFrom, DateTime dateTo, int? top)
        {
            throw new NotImplementedException();
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
                    throw new ExistenceException("No existe un usuario con ese id.");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("No está autorizado para realizar esta acción.");
            }
        }

        //private Func<User, bool> UserById(int id)
        //{
        //    return a => a.Id == id && a.DateDeleted != null;
        //}

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
