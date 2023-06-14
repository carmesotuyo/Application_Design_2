using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.Logging.Logic.Services;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/articles")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class ArticleController : BlogsAppControllerBase
    {
        private readonly IArticleLogic articleLogic;
        private readonly ILoggerService loggerService;

        public ArticleController(IArticleLogic articleLogic, ISessionLogic sessionLogic, ILoggerService loggerService) : base (sessionLogic)
        {
            this.articleLogic = articleLogic;
            this.loggerService = loggerService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? search, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);
            if (search != null) {
                loggerService.LogSearch(loggedUser.Id, search);
            };
            IEnumerable<BasicArticleDto> basicArticleDtos = ArticleConverter.ToDtoList(articleLogic.GetArticles(loggedUser, search));
            return new OkObjectResult(basicArticleDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetArticleById([FromRoute] int id, [FromHeader] string token)
        {
            Article article = articleLogic.GetArticleById(id, base.GetLoggedUser(token));

            return new OkObjectResult(ArticleConverter.ToCompleteDto(article));
        }

        [HttpGet("stats")]
        public IActionResult GetStatsByYear([FromQuery] int year, [FromHeader] string token)
        {
            return new OkObjectResult(articleLogic.GetStatsByYear(year, base.GetLoggedUser(token)));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArticle([FromRoute] int id, [FromHeader] string token)
        {
            articleLogic.DeleteArticle(id, base.GetLoggedUser(token));
            return new OkObjectResult(new { message = "Articulo eliminado" });
        }

        [HttpPost]
        public IActionResult PostArticle([FromBody] BasicArticleDto articleDto, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);
            Article article = ArticleConverter.FromDto(articleDto, loggedUser);
            Article newArticle = articleLogic.CreateArticle(article, loggedUser);
            return new OkObjectResult(ArticleConverter.ToDto(newArticle));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateArticle([FromRoute] int id, [FromBody] UpdateArticleRequestDTO articleRequestDTO, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);
            Article updatedArticle = articleRequestDTO.ApplyChangesToArticle(articleLogic.GetArticleById(id, loggedUser));
            Article newArticle = articleLogic.UpdateArticle(id, updatedArticle, loggedUser);
            return new OkObjectResult(ArticleConverter.ToDto(newArticle));
        }

        [HttpPut("{id}/approval")]
        public IActionResult ApproveArticle([FromRoute] int id,[FromHeader] string token)
        {
            Article articleApproved = articleLogic.ApproveArticle(id, base.GetLoggedUser(token));
            return new OkObjectResult(ArticleConverter.ToDto(articleApproved));
        }
    }
}