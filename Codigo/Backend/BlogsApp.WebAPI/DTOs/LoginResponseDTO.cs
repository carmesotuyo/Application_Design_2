using System;
using BlogsApp.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BlogsApp.WebAPI.DTOs
{
	public class LoginResponseDTO
    {
        public Guid Token { get; set; }
        public IEnumerable<NotificationCommentDto> Comments { get; set; }
        public int UserId { get; set; }

    public LoginResponseDTO( int userId, Guid token, IEnumerable<NotificationCommentDto> comments)
		{
            this.UserId = userId;
            this.Token = token;
            this.Comments = comments;
		}
	}
}

