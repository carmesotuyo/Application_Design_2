using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
	public class UpdateCommentRequestDTO
	{
        public string Body { get; set; }

        public Comment ApplyChangesToComment(Comment comment)
        {
            comment.Body = Body;

            return comment;
        }
    }
}

