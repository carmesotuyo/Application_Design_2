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
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(MessageResponseDTO), 400)]
        [ProducesResponseType(typeof(MessageResponseDTO), 500)]
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
            userLogic.UpdateUser(loggedUser, user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];
            var user = userLogic.DeleteUser(loggedUser, id);
            return Ok(user);
        }

        [HttpGet("/ranking")]
        public IActionResult GetRanking([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, int top)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];
            return Ok(userLogic.GetUsersRanking(loggedUser, dateFrom, dateTo, top));

        }

        [HttpGet("{id}/articles")]
        public IActionResult GetUserArticles([FromRoute] int id)
        {
            User loggedUser = (User)this.HttpContext.Items["user"];
            return Ok(articleLogic.GetArticlesByUser(loggedUser,id));
        }
    }
}