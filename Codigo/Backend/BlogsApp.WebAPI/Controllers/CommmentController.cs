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
            return new OkObjectResult(CommentConverter.toBasicDto(createdCommented));
        }

        [HttpPost("{parentCommentId}")]
        public IActionResult CreateSubCommentFromParent([FromBody] BasicCommentDTO comment, [FromRoute] int parentCommentId, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);

            Article article = articleLogic.GetArticleById(comment.ArticleId, loggedUser);
            Comment parentComment = commentLogic.GetCommentById(parentCommentId, loggedUser);
            Comment newComment = CommentConverter.FromDto(comment, loggedUser, article);
            newComment.isSubComment = true;
            Comment createdComment = commentLogic.ReplyToComment(parentComment, newComment, loggedUser);
            return new OkObjectResult(CommentConverter.toBasicDto(createdComment));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDTO commentRequestDTO, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);
            Comment updatedComment = commentRequestDTO.ApplyChangesToComment(commentLogic.GetCommentById(id, loggedUser));
            Comment newComment = commentLogic.UpdateComment(id, updatedComment, loggedUser);
            return new OkObjectResult(CommentConverter.toBasicDto(newComment));
        }

        [HttpPut("{id}/approval")]
        public IActionResult ApproveComment([FromRoute] int id, [FromHeader] string token)
        {
            Comment commentApproved = commentLogic.ApproveComment(id, base.GetLoggedUser(token));
            return new OkObjectResult(CommentConverter.toBasicDto(commentApproved));
        }
    }
}

