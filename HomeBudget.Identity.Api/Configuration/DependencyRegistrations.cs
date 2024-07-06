using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using HomeBudget.Identity.Api.Constants;
using HomeBudget.Identity.Domain.Models;

namespace HomeBudget.Identity.Api.Configuration
{
    internal static class DependencyRegistrations
    {
        public static IServiceCollection SetUpDi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .SetUpConfigurationOptions(configuration);
        }

        private static IServiceCollection SetUpConfigurationOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .Configure<MongoDbOptions>(configuration.GetSection(ConfigurationSectionKeys.MongoDbOptions));
        }
    }
}
