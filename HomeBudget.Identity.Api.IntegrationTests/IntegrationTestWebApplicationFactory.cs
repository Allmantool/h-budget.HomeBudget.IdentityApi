using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using HomeBudget.Identity.Api.IntegrationTests.Models;
using HomeBudget.Identity.Domain.Constants;
using HomeBudget.Identity.Domain.Models;

namespace HomeBudget.Identity.Api.IntegrationTests
{
    internal class IntegrationTestWebApplicationFactory<TStartup>
        (Func<TestContainersConnections> webHostInitializationCallback) : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private TestContainersConnections _containersConnections;

        internal IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, conf) =>
            {
                conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{HostEnvironments.Integration}.json"));
                conf.AddEnvironmentVariables();

                Configuration = conf.Build();

                _containersConnections = webHostInitializationCallback?.Invoke();
            });

            builder.ConfigureTestServices(services =>
            {
                var mongoDbOptions = new MongoDbOptions
                {
                    ConnectionString = _containersConnections.MongoDbContainer
                };

                services.AddOptions<MongoDbOptions>().Configure(options =>
                {
                    options.ConnectionString = mongoDbOptions.ConnectionString;
                    options.UsersDatabaseName = "user-identity-test";
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}
