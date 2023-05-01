using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;
using BlogsApp.Domain.Exceptions;


namespace BlogsApp.BusinessLogic.Logics
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                throw new BadInputException("Usuario inválido");
            }
            return true;
        }
        //...User LOGIC CODE
    }
}
