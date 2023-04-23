using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.IDataAccess.Interfaces;

namespace BlogsApp.BusinessLogic.Logics
{
    public class SessionLogic : ISessionLogic
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionLogic(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        //...Session LOGIC CODE
    }
}
