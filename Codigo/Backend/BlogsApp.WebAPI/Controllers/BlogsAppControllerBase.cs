using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogsApp.WebAPI.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(ExceptionFilter))]
    public class BlogsAppControllerBase : ControllerBase
    {
        private readonly ISessionLogic sessionLogic;

        public BlogsAppControllerBase(ISessionLogic sessionLogic)
        {
            this.sessionLogic = sessionLogic;
        }

        [NonAction]
        public User GetLoggedUser(string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            return sessionLogic.GetUserFromToken(tokenGuid);
        }
    }
}
