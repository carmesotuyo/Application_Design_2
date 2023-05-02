using System;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/replies")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class ReplyController : BlogsAppControllerBase
    {
        private readonly IReplyLogic replyLogic;

        public ReplyController(IReplyLogic replyLogic)
        {
            this.replyLogic = replyLogic;
        }


        [HttpPost]
        public IActionResult PostReply([FromBody] Reply reply)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];

            return new OkObjectResult(replyLogic.CreateReply(reply, loggedUser));
        }
    }
}

