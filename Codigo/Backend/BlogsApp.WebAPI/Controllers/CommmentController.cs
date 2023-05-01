using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/comments")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class CommmentController : BlogsAppControllerBase
    {
        private readonly ICommentLogic commentLogic;

        public CommmentController(ICommentLogic commentLogic)
        {
            this.commentLogic = commentLogic;
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] Comment comment)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];

            Comment createdCommented = commentLogic.CreateComment(comment, loggedUser);
            return new OkObjectResult(createdCommented);

        }
    }
}

