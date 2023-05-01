using System;

namespace BlogsApp.Domain.Exceptions
{
    [Serializable]
    public class InterruptedActionException : Exception
    {
        public InterruptedActionException(string message) : base(message)
        {
        }
    }
}