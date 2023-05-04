using BlogsApp.Domain.Entities;

namespace BlogsApp.WebAPI.DTOs
{
    public class ReplyConverter
    {
        public static Reply FromDto(BasicReplyDTO dto, User user)
        {
            return new Reply(user, dto.Body);
        }

        public static ReplyDTO toDto(Reply reply, User user)
        {
            return new ReplyDTO() {
                Id = reply.Id,
                User = new BasicUserDTO(user),
                Body = reply.Body,
                DateCreated = reply.DateCreated,
                DateDeleted = reply.DateDeleted
            };
        }

        public static BasicReplyDTO toBasicDto(Reply reply)
        {
            return new BasicReplyDTO()
            {
                Body = reply.Body
            };
        }


        public static IEnumerable<ReplyDTO> ToDtoList(IEnumerable<Reply> replies)
        {
            List<ReplyDTO> repliesDtos = new List<ReplyDTO>();
            foreach (Reply reply in replies)
            {
                repliesDtos.Add(toDto(reply, reply.User));
            }
            return repliesDtos;
        }
    }
}
