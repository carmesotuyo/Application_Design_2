using System;

namespace BlogsApp.Domain.Exceptions
{
    [Serializable]
    public class NonExistantImplementationException : Exception
    {
        public NonExistantImplementationException()
        {
        }
    }
}