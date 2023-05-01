using System;

namespace BlogsApp.DataAccess.Interfaces.Exceptions
{
    [Serializable]
    public class AlreadyExistsDbException : Exception
    {
        public AlreadyExistsDbException() { }
    }
}