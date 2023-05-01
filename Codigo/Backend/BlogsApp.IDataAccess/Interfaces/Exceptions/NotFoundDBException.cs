using System;

namespace BlogsApp.DataAccess.Interfaces.Exceptions
{
    [Serializable]
    public class NotFoundDbException : Exception
    {
        public NotFoundDbException() { }

    }
}