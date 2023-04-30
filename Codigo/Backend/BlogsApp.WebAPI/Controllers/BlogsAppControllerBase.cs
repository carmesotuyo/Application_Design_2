using BlogsApp.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlogsApp.WebAPI.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(ExceptionFilter))]
    public class BlogsAppControllerBase : ControllerBase
    {
    }
}
