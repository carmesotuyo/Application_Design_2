using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
	public class LoginResponseDTO
    {
        public Guid Token { get; set; }
        public IEnumerable<BasicCommentDTO> Comments { get; set; }

        public LoginResponseDTO( Guid token, IEnumerable<BasicCommentDTO> comments)
		{
            this.Token = token;
            this.Comments = comments;
		}
	}
}

