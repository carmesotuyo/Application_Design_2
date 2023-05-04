using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.Logging.Logic.Services;
using BlogsApp.BusinessLogic.Logics;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/articles")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class ArticleController : BlogsAppControllerBase
    {
        private readonly IArticleLogic articleLogic;
        private readonly ISessionLogic sessionLogic;
        private readonly ILoggerService loggerService;

        public ArticleController(IArticleLogic articleLogic, ISessionLogic sessionLogic, ILoggerService loggerService)
        {
            this.articleLogic = articleLogic;
            this.sessionLogic = sessionLogic;
            this.loggerService = loggerService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? search, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            if (search != null) {
                loggerService.LogSearch(loggedUser.Id, search);
            };
            IEnumerable<BasicArticleDto> basicArticleDtos = ArticleConverter.ToDtoList(articleLogic.GetArticles(loggedUser, search));
            return new OkObjectResult(basicArticleDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetArticleById([FromRoute] int id, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            Article article = articleLogic.GetArticleById(id, loggedUser);
            return new OkObjectResult(ArticleConverter.ToDto(article));
        }

        [HttpGet("stats")]
        public IActionResult GetStatsByYear([FromQuery] int year, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            return new OkObjectResult(articleLogic.GetStatsByYear(year, loggedUser));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArticle([FromRoute] int id, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            articleLogic.DeleteArticle(id, loggedUser);
            return new OkObjectResult("Artículo eliminado correctamente");
        }

        [HttpPost]
        public IActionResult PostArticle([FromBody] BasicArticleDto articleDto, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            Article article = ArticleConverter.FromDto(articleDto, loggedUser);
            Article newArticle = articleLogic.CreateArticle(article, loggedUser);
            return new OkObjectResult(ArticleConverter.ToDto(newArticle));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateArticle([FromRoute] int id, [FromBody] UpdateArticleRequestDTO articleRequestDTO, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            Article updatedArticle = articleRequestDTO.ApplyChangesToArticle(articleLogic.GetArticleById(id, loggedUser));
            Article newArticle = articleLogic.UpdateArticle(id, updatedArticle, loggedUser);
            return new OkObjectResult(ArticleConverter.ToDto(newArticle));
        }
    }
}