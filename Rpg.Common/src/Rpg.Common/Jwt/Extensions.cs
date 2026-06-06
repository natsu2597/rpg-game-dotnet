using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rpg.Common.Settings;


namespace Rpg.Common.Jwt
{
    public static class Extensions
    {
        public static IServiceCollection AddJwt(
                this IServiceCollection services,
                IConfiguration configuration
            )

        {
            services.Configure<JwtSettings>(
                configuration.GetSection(nameof(JwtSettings))
    );

            return services;
        }
    }
}
