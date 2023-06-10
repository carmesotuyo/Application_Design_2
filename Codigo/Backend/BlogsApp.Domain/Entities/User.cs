namespace BlogsApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get => _username; set { checkUsernameValid(value); _username = value; } }
        public string Password { get; set; }
        public string Email { get => _email; set { checkEmailValid(value); _email = value; } }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Blogger { get; set; }
        public bool Admin { get; set; }
        public bool Moderador { get; set; }
        public DateTime? DateDeleted { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }

        private string _username;
        private string _email;

        public User(string username, string password, string email, string name, string lastName, bool blogger, bool admin, bool moderador)    
        {
            Username = username;
            Password = password;
            Email = email;
            Name = name;
            LastName = lastName;
            Blogger = blogger;
            Admin = admin;
            Moderador = moderador;
            Articles = new List<Article>();
            Comments = new List<Comment>();
        }

        public User() { }

        private void checkUsernameValid(string username)
        {
            int originalLength = username.Length;
            string removeSpaces = username.Replace(" ", "");
            int lengthWithoutSpaces = removeSpaces.Length;
            if (lengthWithoutSpaces < originalLength || originalLength < 1 || originalLength > 12)
            {
                throw new InvalidDataException("Username no tiene el formato correcto - sin espacios, 12 caracteres máximo");
            }
        }

        private void checkEmailValid(string email)
        {
            email.Trim();
            string[] emailSections = email.Split('@');
            bool hasUser = false;
            bool hasDomain = false;

            if (emailSections.Length == 2)
            {
                hasUser = emailSections[0].Length > 0;
                hasDomain = emailSections[1].EndsWith(".com");
            }

            if (!hasUser || !hasDomain)
            {
                throw new InvalidDataException("Email no tiene el formato correcto - debe incluir '@' y '.com'");
            }
        }
    }
}
