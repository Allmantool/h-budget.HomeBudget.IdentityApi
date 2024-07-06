using Microsoft.Extensions.DependencyInjection;

using HomeBudget.Components.Users.Clients;
using HomeBudget.Components.Users.Clients.Interfaces;
using HomeBudget.Components.Users.Services;
using HomeBudget.Components.Users.Services.Interfaces;

namespace HomeBudget.Components.Users.Configuration
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterApiIoCDependency(
            this IServiceCollection services)
        {
            return services
                .AddScoped<IUsersService, UsersService>()
                .RegisterMongoDbClient();
        }

        private static IServiceCollection RegisterMongoDbClient(this IServiceCollection services)
        {
            return services.AddSingleton<IUserDocumentsClient, UserDocumentsClient>();
        }
    }
}
