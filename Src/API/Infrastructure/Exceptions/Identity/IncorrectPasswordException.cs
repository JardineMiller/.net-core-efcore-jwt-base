using System;

namespace API.Infrastructure.Exceptions.Identity
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message) : base(message) {}

    }
}
