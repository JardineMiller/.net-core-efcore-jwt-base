using System.Security.Claims;
using API.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace API.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.user = httpContextAccessor.HttpContext?.User;
        }

        public string GetUserName()
        {
            return this.user?.Identity?.Name;
        }

        public string GetId()
        {
            return this.user?.GetId();
        }
    }
}
