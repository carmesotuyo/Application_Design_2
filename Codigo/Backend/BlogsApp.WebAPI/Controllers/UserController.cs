using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogsApp.WebAPI.DTOs;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/users")]
    public class UserController: BlogsAppControllerBase
    {
        private readonly IUserLogic userLogic;
        public UserController(IUserLogic UserLogic) 
        {
            this.userLogic = UserLogic;
        }

        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(MessageResponseDTO), 400)]
        [ProducesResponseType(typeof(MessageResponseDTO), 500)]
        public IActionResult PostUser([FromBody] CreateUserRequestDTO userDTO)
        {
            MessageResponseDTO response = new MessageResponseDTO(true, "");
            return Ok(userLogic.InsertUser(userDTO.TransformToUser()));
        }

        //public IActionResult PatchUser([FromBody] User aUser)
        //{
        //    //return new OkObjectResult;
        //}

        //public IActionResult DeleteUser([FromBody] User aUser)
        //{
        //    //return new OkObjectResult;
        //}
    }
}
