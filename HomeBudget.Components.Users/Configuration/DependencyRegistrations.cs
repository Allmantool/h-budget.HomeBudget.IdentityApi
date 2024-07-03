using Microsoft.Extensions.DependencyInjection;

using HomeBudget.Components.Users.Clients;
using HomeBudget.Components.Users.Clients.Interfaces;
using HomeBudget.Components.Users.Services;
using HomeBudget.Components.Users.Services.Interfaces;

namespace HomeBudget.Components.Users.Configuration
{
    public static class DependencyRegistrations
    {
        public static IServiceCollection RegisterUsersIoCDependency(
            this IServiceCollection services)
        {
            return services
                .AddScoped<IUsersService, UsersService>()
                .AddScoped<IUserDocumentsClient, UserDocumentsClient>();
        }
    }
}
