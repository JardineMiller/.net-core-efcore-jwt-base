using System;
using API.Data.Models;

namespace API.Infrastructure.Exceptions
{
    public class UserAccessViolation : Exception
    {
        public UserAccessViolation(string name, string userId) : base($"{name} does not belong to {nameof(User)} {userId}") { }
    }
}
