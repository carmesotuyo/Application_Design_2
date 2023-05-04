﻿using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class CommentConverter
    {
        public static CommentDTO toDto(Comment comment)
        {
            CommentDTO commentDto = new CommentDTO()
            {
                Id = comment.Id,
                User = new BasicUserDTO(comment.User),
                ArticleId = comment.Article.Id,
                Body = comment.Body,
                DateCreated = comment.DateCreated,
                DateDeleted = comment.DateDeleted
            };

            if (comment.Reply != null)
            {
                ReplyDTO replyDto = ReplyConverter.toDto(comment.Reply, comment.Reply.User);
                commentDto.Reply = replyDto;
            }


            return commentDto;
        }

        public static BasicCommentDTO toBasicDto(Comment comment)
        {
            return new BasicCommentDTO()
            {
                Body = comment.Body,
                ArticleId = comment.Article.Id
            };
        }

        public static Comment FromDto(BasicCommentDTO dto, User user, Article article)
        {
            return new Comment(user, dto.Body, article);
        }


        public static IList<CommentDTO> ToDtoList(ICollection<Comment> comments)
        {
            List<CommentDTO> commentsDtos = new List<CommentDTO>();
            foreach (Comment comment in comments)
            {
                commentsDtos.Add(toDto(comment));
            }
            return commentsDtos;
        }
    }
}
