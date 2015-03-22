using System;

namespace Api.App.Exceptions
{
    public class InvalidPotoStateException : Exception
    {
        public InvalidPotoStateException()
        {
        }

        public InvalidPotoStateException(string message) : base(message)
        {
        }
    }
}