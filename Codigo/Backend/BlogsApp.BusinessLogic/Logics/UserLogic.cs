using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //...User LOGIC CODE
    }
}
