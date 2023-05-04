using System;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/replies")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class ReplyController : BlogsAppControllerBase
    {
        private readonly IReplyLogic replyLogic;
        private readonly ISessionLogic sessionLogic;

        public ReplyController(IReplyLogic replyLogic, ISessionLogic sessionLogic)
        {
            this.replyLogic = replyLogic;
            this.sessionLogic = sessionLogic;
        }


        [HttpPost]
        public IActionResult PostReply([FromBody] BasicReplyDTO reply, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);

            Reply newReply = ReplyConverter.FromDto(reply, loggedUser);
            Reply createdReply = replyLogic.CreateReply(newReply, loggedUser);
            return new OkObjectResult(ReplyConverter.toBasicDto(createdReply));
        }
    }
}

