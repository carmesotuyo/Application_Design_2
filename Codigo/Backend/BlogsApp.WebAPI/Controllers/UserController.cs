using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.DTOs;
using BlogsApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Exceptions;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/users")]
    public class UserController : BlogsAppControllerBase
    {

        private readonly IUserLogic userLogic;
        public UserController(IUserLogic userLogic)
        {
            this.userLogic = userLogic;
        }

        //// GET: api/User
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(typeof(MessageResponseDTO), 500)]
        public IActionResult GetUsers()
        {
            MessageResponseDTO response = new MessageResponseDTO(true, "");
            try
            {
                return Ok(userLogic.GetUsers());
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Algo salió mal.";
                return StatusCode(500, response);
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(MessageResponseDTO), 400)]
        [ProducesResponseType(typeof(MessageResponseDTO), 500)]
        public IActionResult PostUser([FromBody] User user)
        {
            MessageResponseDTO response = new MessageResponseDTO(true, "");
            try
            {
                return Ok(userLogic.InsertUser(user));
            }
            catch (BusinessLogicException exception)
            {
                response.Success = false;
                response.Message = exception.Message;
                return BadRequest(response);
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Algo salió mal.";
                return StatusCode(500, response);
            }
        }

        //// GET: api/User/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/User
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/User/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/User/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
