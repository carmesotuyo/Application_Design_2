using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.BusinessLogic.Logics;
using NuGet.Protocol.Plugins;

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
            Guid token = sessionLogic.Login(username, password);
            User user = sessionLogic.GetUserFromToken(token);
            IEnumerable<Comment> comments = sessionLogic.GetCommentsWhileLoggedOut(user.Id);

            var response = new LoginResponseDTO(token, comments);

            return Ok(response);
        }

    }
}

