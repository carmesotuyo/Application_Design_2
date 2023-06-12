using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/comments")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class CommmentController : BlogsAppControllerBase
    {
        private readonly ICommentLogic commentLogic;
        private readonly IArticleLogic articleLogic;

        public CommmentController(ICommentLogic commentLogic, ISessionLogic sessionLogic, IArticleLogic articleLogic) : base (sessionLogic)
        {
            this.commentLogic = commentLogic;
            this.articleLogic = articleLogic;
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] BasicCommentDTO comment, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);

            Article article = articleLogic.GetArticleById(comment.ArticleId, loggedUser);
            Comment newComment = CommentConverter.FromDto(comment, loggedUser, article);
            Comment createdCommented = commentLogic.CreateComment(newComment, loggedUser);
            return new OkObjectResult(CommentConverter.toDto(createdCommented));
        }

        [HttpPost("{parentCommentId}")]
        public IActionResult CreateSubCommentFromParent([FromBody] BasicCommentDTO comment, [FromRoute] int parentCommentId, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);

            Article article = articleLogic.GetArticleById(comment.ArticleId, loggedUser);
            Comment parentComment = commentLogic.GetCommentById(parentCommentId);
            Comment newComment = CommentConverter.FromDto(comment, loggedUser, article);
            Comment createdComment = commentLogic.ReplyToComment(parentComment, newComment, loggedUser);
            return new OkObjectResult(CommentConverter.toDto(createdComment));
        }
    }
}

