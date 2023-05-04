using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.DTOs;
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
        private readonly IArticleLogic articleLogic;

        public CommmentController(ICommentLogic commentLogic, ISessionLogic sessionLogic, IArticleLogic articleLogic)
        {
            this.commentLogic = commentLogic;
            this.sessionLogic = sessionLogic;
            this.articleLogic = articleLogic;
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] CommentDTO comment, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);

            Article article = articleLogic.GetArticleById(comment.ArticleId, loggedUser);
            Comment createdCommented = commentLogic.CreateComment(new Comment(loggedUser, comment.Body, article), loggedUser);
            return new OkObjectResult(CommentConverter.toDto(createdCommented));

        }
    }
}

