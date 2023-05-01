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

        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User CreateUser(User user)
        {
            IsUserValid(user);
            _userRepository.Add(user!);
            return user;
        }

        public User DeleteUser1(int userId)
        {
            User user = _userRepository.Get(UserById(userId));
            user.DateDeleted = DateTime.Now;
            _userRepository.Update(user);
            return user;
        }

        public User GetUserById(int userId)
        {
            return _userRepository.Get(m => m.DateDeleted == null && m.Id == userId);
        }

        public User DeleteUser(User loggedUser, int UserId)
        {
            if (_userRepository.Exists(m => m.Id == UserId))
            {
                User user = GetUserById(UserId);
                user.DateDeleted = DateTime.Now;
                _userRepository.Update(user);
                return user;
            }
            else
            {
                throw new ExistenceException ("No existe un usuario con ese id.");
            }
        }

        public ICollection<User> GetUsersRanking(User loggedUser, int? top)
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

        public User? UpdateUser(User loggedUser, User user)
        {
            _userRepository.Update(user!);
            return user;
        }

        private Func<User, bool> UserById(int id)
        {
            return a => a.Id == id && a.DateDeleted != null;
        }

    }
}
