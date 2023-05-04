using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.DTOs;
using Azure.Core;
using System.Data;
using BlogsApp.BusinessLogic.Logics;
using NuGet.Common;
using Newtonsoft.Json.Linq;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/users")]
    public class UserController: BlogsAppControllerBase
    {
        private readonly IUserLogic userLogic;
        private readonly IArticleLogic articleLogic;
        private readonly ISessionLogic sessionLogic;
        public UserController(IUserLogic userLogic, IArticleLogic articleLogic, ISessionLogic sessionLogic) 
        {
            this.userLogic = userLogic;
            this.articleLogic = articleLogic;
            this.sessionLogic = sessionLogic;
        }

        [HttpPost]
        public IActionResult PostUser([FromBody] CreateUserRequestDTO userDTO)
        {
            MessageResponseDTO response = new MessageResponseDTO(true, "");
            return Ok(userLogic.CreateUser(userDTO.TransformToUser()));
        }


        [HttpPatch("{id}")]
        public IActionResult PatchUser([FromRoute] int id, [FromBody] UpdateUserRequestDTO userDTO, [FromHeader] string token)
        {
            var user = userLogic.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            user = userDTO.ApplyChangesToUser(user);
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);

            return new OkObjectResult(userLogic.UpdateUser(loggedUser, user));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromRoute] int id, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);

            return new OkObjectResult(userLogic.DeleteUser(loggedUser, id));
        }

        [HttpGet("ranking")]
        public IActionResult GetRanking([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] int? top, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            return new OkObjectResult(userLogic.GetUsersRanking(loggedUser, dateFrom, dateTo, top));

        }

        [HttpGet("{id}/articles")]
        public IActionResult GetUserArticles([FromRoute] int id, [FromHeader] string token)
        {
            Guid tokenGuid = Guid.Parse(token);
            User loggedUser = sessionLogic.GetUserFromToken(tokenGuid);
            IEnumerable<BasicArticleDto> basicArticleDtos = ArticleConverter.ToDtoList(articleLogic.GetArticlesByUser(id, loggedUser));
            return new OkObjectResult(basicArticleDtos);
        }
    }
}