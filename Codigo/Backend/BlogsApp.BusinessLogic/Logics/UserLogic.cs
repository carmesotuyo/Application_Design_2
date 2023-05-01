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

        public User CreateUser(User user)
        {
            IsUserValid(user);
            this._userRepository.Add(user!);
            return user;
        }

        public User DeleteUser(int userId)
        {
            User user = _userRepository.Get(UserById(userId));
            user.DateDeleted = DateTime.Now;
            this._userRepository.Update(user);
            return user;
        }

        public User GetUserById(int userId)
        {
            return _userRepository.Get(m => m.DateDeleted == null && m.Id == userId);
        }

        public ICollection<User> GetUsersRanking(int? top)
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

        public User? UpdateUser(int userId, User anUser)
        {
            User user = _userRepository.Get(UserById(userId));
            user.Name = anUser.Name;
            user.Email = anUser.Email;
            user.Admin = anUser.Admin;
            user.Blogger = anUser.Blogger;
            user.LastName = anUser.LastName;
            user.Password = anUser.Password;
            this._userRepository.Update(user);
            return user;
        }

        private Func<User, bool> UserById(int id)
        {
            return a => a.Id == id && a.DateDeleted != null;
        }

    }
}
