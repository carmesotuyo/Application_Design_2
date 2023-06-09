using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.BusinessLogic.Logics;
using NuGet.Protocol.Plugins;
using NuGet.Common;
using BlogsApp.Logging.Logic.Services;

namespace BlogsApp.WebAPI.Controllers
{
	[Route("api/sessions")]
    public class SessionController : BlogsAppControllerBase
    {
        private readonly ISessionLogic sessionLogic;
        private readonly ILoggerService _loggerService;

        public SessionController(ISessionLogic sessionLogic, ILoggerService loggerService) : base(sessionLogic)
        {
            this.sessionLogic = sessionLogic;
            _loggerService = loggerService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO credentials)
        {
            Guid token = sessionLogic.Login(credentials.Username, credentials.Password);
            User user = sessionLogic.GetUserFromToken(token);
            _loggerService.LogLogin(user.Id);

            var response = new LoginResponseDTO(token, GetAndConvertCommentsToNotify(user));

            return Ok(response);
        }

        private IEnumerable<NotificationCommentDto> GetAndConvertCommentsToNotify(User user)
        {
            List<NotificationCommentDto> comments = new List<NotificationCommentDto>();

            foreach (Comment comment in sessionLogic.GetCommentsWhileLoggedOut(user))
            {
                comments.Add(CommentConverter.toNotificationDto(comment));
            }
            return comments;
        }

        [ServiceFilter(typeof(AuthorizationFilter))]
        [HttpPatch]
        public IActionResult Logout([FromHeader] string token)
        {
            sessionLogic.Logout(base.GetLoggedUser(token));

            return new OkObjectResult("Usuario deslogueado correctamente");
        }

    }
}

