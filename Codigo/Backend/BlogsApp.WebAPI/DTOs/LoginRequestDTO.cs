using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
	public class LoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginRequestDTO(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}

