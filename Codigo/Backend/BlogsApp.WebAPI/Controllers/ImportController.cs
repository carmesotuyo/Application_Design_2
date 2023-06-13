using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.Filters;
using BlogsApp.Domain.Entities;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.Logging.Logic.Services;
using BlogsApp.BusinessLogic.Logics;
using NuGet.Common;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/articles")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class ImportController : BlogsAppControllerBase
    {
        private readonly IArticleLogic articleLogic;
        private readonly ILoggerService loggerService;
        private readonly IImporterLogic importerLogic;

        public ImportController(IArticleLogic articleLogic, ISessionLogic sessionLogic, ILoggerService loggerService, IImporterLogic importerLogic) : base (sessionLogic)
        {
            this.articleLogic = articleLogic;
            this.loggerService = loggerService;
            this.importerLogic = importerLogic;
        }

        [HttpGet("importers")]
        public IActionResult GetImporters()
        {
           List<string> retrievedImporters = importerLogic.GetAllImporters();
           Console.WriteLine(retrievedImporters);
           return Ok(retrievedImporters);
        }

        [HttpPost("import")]
        public IActionResult ImportArticles([FromBody] ImporterInformation importerInformation, [FromHeader] string token)
        {
            User loggedUser = base.GetLoggedUser(token);
            importerLogic.ImportArticles(importerInformation.ImporterName, importerInformation.Path, loggedUser);
            return new OkObjectResult(new { message = "Articulos importados correctamente" });
        }
    }
}