using System;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.Logging.Logic.Services;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/logs")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class LogController : BlogsAppControllerBase
    {
        private readonly ILoggerService loggerService;

        public LogController(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];
            return new OkObjectResult(loggerService.GetLogs(from, to, loggedUser));
        }
    }
}

