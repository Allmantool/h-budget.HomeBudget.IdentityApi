using Microsoft.Extensions.DependencyInjection;

using HomeBudget.Identity.Domain.Interfaces;

namespace HomeBudget.Identity.Domain.Configuration
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterUsersIoCDependency(
            this IServiceCollection services)
        {
            return services
                .AddScoped<IJwtBuilder, JwtBuilder>()
                .AddScoped<IEncryptor, Encryptor>();
        }
    }
}
