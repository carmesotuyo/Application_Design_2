using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.BusinessLogic.Logics;
using NuGet.Protocol.Plugins;
using NuGet.Common;

namespace BlogsApp.WebAPI.Controllers
{
	[Route("api/sessions")]
    public class SessionController : BlogsAppControllerBase
    {
        private readonly ISessionLogic sessionLogic;

        public SessionController(ISessionLogic sessionLogic)
        {
            this.sessionLogic = sessionLogic;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO credentials)
        {
            Guid token = sessionLogic.Login(credentials.Username, credentials.Password);
            User user = sessionLogic.GetUserFromToken(token);
            IEnumerable<Comment> comments = sessionLogic.GetCommentsWhileLoggedOut(user.Id);

            var response = new LoginResponseDTO(token, comments);

            return Ok(response);
        }

        [ServiceFilter(typeof(AuthorizationFilter))]
        [HttpPatch("{id}")]
        public IActionResult Logout([FromRoute] int id, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            sessionLogic.Logout(id, loggedUser);

            return new OkObjectResult("Usuario deslogueado correctamente");
        }

    }
}

