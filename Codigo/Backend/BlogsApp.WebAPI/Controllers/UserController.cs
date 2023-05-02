using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.DTOs;
using Azure.Core;
using System.Data;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/users")]
    public class UserController: BlogsAppControllerBase
    {
        private readonly IUserLogic userLogic;
        private readonly IArticleLogic articleLogic;
        public UserController(IUserLogic userLogic, IArticleLogic articleLogic) 
        {
            this.userLogic = userLogic;
            this.articleLogic = articleLogic;
        }

        [HttpPost]
        public IActionResult PostUser([FromBody] CreateUserRequestDTO userDTO)
        {
            MessageResponseDTO response = new MessageResponseDTO(true, "");
            return Ok(userLogic.CreateUser(userDTO.TransformToUser()));
        }


        [HttpPatch("{id}")]
        public IActionResult PatchUser([FromRoute] int id, [FromBody] UpdateUserRequestDTO userDTO)
        {
            var user = userLogic.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            user = userDTO.ApplyChangesToUser(user);
            User loggedUser = (User)this.HttpContext.Items["user"];

            return new OkObjectResult(userLogic.UpdateUser(loggedUser, user));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];

            return new OkObjectResult(userLogic.DeleteUser(loggedUser, id));
        }

        [HttpGet("ranking")]
        public IActionResult GetRanking([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] int? top)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];
            return new OkObjectResult(userLogic.GetUsersRanking(loggedUser, dateFrom, dateTo, top));

        }

        [HttpGet("{id}/articles")]
        public IActionResult GetUserArticles([FromRoute] int id)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];
            return new OkObjectResult(articleLogic.GetArticlesByUser(id,loggedUser));
        }
    }
}