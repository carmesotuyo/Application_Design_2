﻿namespace BlogsApp.WebAPI.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public BasicUserDTO User { get; set; }
        public ReplyDTO Reply { get; set; }
        public int ArticleId { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}