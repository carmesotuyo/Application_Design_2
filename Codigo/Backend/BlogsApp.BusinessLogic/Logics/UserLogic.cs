using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using Exceptions;

namespace BlogsApp.BusinessLogic.Logics
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User>? users = _userRepository.GetUsers();
            if (users == null) return new List<User>();
            return users;
        }

        public User? InsertUser(User? user)
        {
            if (IsUserValid(user))
            {
                _userRepository.InsertUser(user!);
            }
            return user;
        }

        public bool IsUserValid(User? user)
        {
            if (user == null)
            {
                throw new BusinessLogicException("Usuario inválido");
            }
            return true;
        }

        //...User LOGIC CODE
    }
}
