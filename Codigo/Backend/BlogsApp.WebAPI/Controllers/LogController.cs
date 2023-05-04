using System;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.Logging.Logic.Services;
using NuGet.Common;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/logs")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class LogController : BlogsAppControllerBase
    {
        private readonly ILoggerService loggerService;
        private readonly ISessionLogic sessionLogic;

        public LogController(ILoggerService loggerService, ISessionLogic sessionLogic)
        {
            this.loggerService = loggerService;
            this.sessionLogic = sessionLogic;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] DateTime from, [FromQuery] DateTime to, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            return new OkObjectResult(loggerService.GetLogs(from, to, loggedUser));
        }
    }

}

