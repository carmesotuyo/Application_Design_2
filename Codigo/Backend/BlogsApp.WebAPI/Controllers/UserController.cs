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
        public UserController(IUserLogic userLogic) 
        {
            this.userLogic = userLogic;
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
            userLogic.UpdateUser(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            var user = userLogic.DeleteUser(id);
            return Ok(user);
        }
    }
}
