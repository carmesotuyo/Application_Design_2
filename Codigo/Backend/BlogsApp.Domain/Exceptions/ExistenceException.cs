using System;

namespace BlogsApp.Domain.Exceptions
{
    [Serializable]
    public class ExistenceException : Exception
    {
        public ExistenceException(string message) : base(message)
        {
        }
    }
}