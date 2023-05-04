using System;

namespace BlogsApp.Domain.Exceptions
{
    [Serializable]
    public class BadInputException : Exception
    {
        public BadInputException(string message) : base(message)
        {
        }

    }
}