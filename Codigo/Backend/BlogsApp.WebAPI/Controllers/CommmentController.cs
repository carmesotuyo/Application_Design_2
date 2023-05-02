using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/comments")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class CommmentController : BlogsAppControllerBase
    {
        private readonly ICommentLogic commentLogic;
        private readonly ISessionLogic sessionLogic;

        public CommmentController(ICommentLogic commentLogic, ISessionLogic sessionLogic)
        {
            this.commentLogic = commentLogic;
            this.sessionLogic = sessionLogic;
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] Comment comment, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);

            Comment createdCommented = commentLogic.CreateComment(comment, loggedUser);
            return new OkObjectResult(createdCommented);

        }
    }
}

