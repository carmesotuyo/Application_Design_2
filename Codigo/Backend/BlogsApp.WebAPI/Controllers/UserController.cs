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

        public UserController(IUserLogic userLogic, IArticleLogic articleLogic, ISessionLogic sessionLogic) : base(sessionLogic)
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
        public IActionResult PatchUser([FromRoute] int id, [FromBody] UserDto userDTO, [FromHeader] string token)
        {
            var user = userLogic.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            User updateUser = UserConverter.ToUser(userDTO, id, user);
            User userlogged = (base.GetLoggedUser(token));
            User updatedUser = userLogic.UpdateUser(userlogged, updateUser);
            return new OkObjectResult(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromRoute] int id, [FromHeader] string token)
        {
            return new OkObjectResult(userLogic.DeleteUser(base.GetLoggedUser(token), id));
        }

        [HttpGet("ranking")]
        public IActionResult GetRanking([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] int? top, [FromQuery] bool? withOffensiveWords, [FromHeader] string token)
        {
            ICollection<User> ranking = userLogic.GetUsersRanking(base.GetLoggedUser(token), dateFrom, dateTo, top, withOffensiveWords);
            ICollection<UserRankingDto> rankingResponse = UserRankingConverter.ToRankingList(ranking);
            return new OkObjectResult(rankingResponse);

        }

        [HttpGet("{id}/articles")]
        public IActionResult GetUserArticles([FromRoute] int id, [FromHeader] string token)
        {
            IEnumerable<BasicArticleDto> basicArticleDtos = ArticleConverter.ToDtoList(articleLogic.GetArticlesByUser(id, base.GetLoggedUser(token)));
            return new OkObjectResult(basicArticleDtos);
        }

        [HttpGet]
        public IActionResult GetUsers([FromHeader] string token)
        {
            IEnumerable<UserDto> userDtos = UserConverter.ToDtoList(userLogic.GetUsers(base.GetLoggedUser(token)));
            return new OkObjectResult(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById([FromHeader] string token, [FromRoute] int id)
        {
            UserDto userDto = UserConverter.ToDto(userLogic.GetUserById(id));
            return new OkObjectResult(userDto);
        }
    }
}