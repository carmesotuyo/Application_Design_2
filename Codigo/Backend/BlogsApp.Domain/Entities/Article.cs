using BlogsApp.Domain.Exceptions;
using BlogsApp.Domain.Enums;

namespace BlogsApp.Domain.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool Private { get; set; }
        public ContentState State { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public int Template { get; set; }
        public string? Image { get; set; }


        public Article(string name, string body, int template, User user)
        {
            Name = name;
            Body = body;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
            User = user;
            UserId = user.Id;
            Comments = new List<Comment>();
            Template = template;
        }

        public Article() { }

        public void IsValid()
        {
            if (Name == null || Name.Trim() == "") { throw new BadInputException("Debe ingresar un nombre."); }
            if (Body == null || Body.Trim() == "") { throw new BadInputException("Debe ingresar un contenido."); }
        }
        public override bool Equals(object obj)
        {
            var item = obj as Article;

            if (item == null)
            {
                return false;
            }

            return Id.Equals(item.Id);
        }
    }
}

