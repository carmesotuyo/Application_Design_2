using System;
using BlogsApp.BusinessLogic.Logics;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/logs")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class LogController : BlogsAppControllerBase
    {
        private readonly ILogLogic logLogic;

        public LogController(ILogLogic logLogic)
        {
            this.logLogic = logLogic;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];
            return new OkObjectResult(logLogic.GetLogs(loggedUser, from, to));
        }
    }
}

