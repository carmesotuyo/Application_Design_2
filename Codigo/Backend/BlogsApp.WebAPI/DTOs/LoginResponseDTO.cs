using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
	public class LoginResponseDTO
    {
        public Guid Token { get; set; }
        public IEnumerable<NotificationCommentDto> Comments { get; set; }

        public LoginResponseDTO( Guid token, IEnumerable<NotificationCommentDto> comments)
		{
            this.Token = token;
            this.Comments = comments;
		}
	}
}

