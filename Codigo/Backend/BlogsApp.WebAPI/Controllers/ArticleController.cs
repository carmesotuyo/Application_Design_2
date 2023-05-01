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
            User loggedUser = (User)this.HttpContext.Items["user"];
            return new OkObjectResult(articleLogic.GetArticles(loggedUser, search));
        }

        [HttpGet("{id}")]
        public IActionResult GetArticleById([FromRoute] int id)
        {
            return new OkObjectResult(articleLogic.GetArticleById(id));
        }

        [HttpGet("/stats")]
        public IActionResult GetStatsByYear([FromQuery]int year)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];

            return new OkObjectResult(articleLogic.GetStatsByYear(year, loggedUser));
        }
    }
}