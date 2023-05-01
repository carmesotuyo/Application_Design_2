using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/articles")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class ArticleController : BlogsAppControllerBase
    {
        private readonly IArticleLogic articleLogic;

        public ArticleController(IArticleLogic articleLogic)
        {
            this.articleLogic = articleLogic;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? search)
        {
            return new OkObjectResult(articleLogic.GetArticles(search));
        }

        [HttpGet("{id}")]
        public IActionResult GetArticleById([FromRoute] int id)
        {
            return new OkObjectResult(articleLogic.GetArticleById(id));
        }

        [HttpGet("{userId}")]
        public IActionResult GetByUser([FromRoute]int userId)
        {
            return new OkObjectResult(articleLogic.GetArticlesByUser(userId));
        }

        [HttpGet("/stats")]
        public IActionResult GetStatsByYear([FromQuery]int year)
        {
            return new OkObjectResult(articleLogic.GetStatsByYear(year));
        }
    }
}