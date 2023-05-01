using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.BusinessLogic.Logics;

namespace BlogsApp.WebAPI.Controllers
{
	[Route("api/sessions")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class SessionController : BlogsAppControllerBase
    {
        private readonly ISessionLogic sessionLogic;

        public SessionController(ISessionLogic sessionLogic)
        {
            this.sessionLogic = sessionLogic;
        }

        [HttpPost]
        public IActionResult Login([FromBody] string username, [FromBody] string password)
        {
            return new OkObjectResult(sessionLogic.Login(username, password));
        }

    }
}

