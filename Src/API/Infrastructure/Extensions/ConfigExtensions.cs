using Microsoft.Extensions.Configuration;

namespace API.Infrastructure.Extensions
{
    public static class ConfigExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration config)
        {
            return config.GetConnectionString("DefaultConnection");
        }


    }
}
