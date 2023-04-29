using System;
using BlogsApp.Domain.Entities;
namespace BlogsApp.WebAPI.DTOs
{
	public class CreateUserRequestDTO
	{
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }



        public CreateUserRequestDTO() { }

        //public User TransformToUser()
        //{
        //    return new User(
        //        this.Username,
        //        this.Password,
        //        this.Name,
        //        this.LastName
        //    );
        //}
    }
}

