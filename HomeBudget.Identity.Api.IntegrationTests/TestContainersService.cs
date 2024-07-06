using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Testcontainers.MongoDb;

namespace HomeBudget.Identity.Api.IntegrationTests
{
    internal class TestContainersService(IConfiguration configuration) : IAsyncDisposable
    {
        public MongoDbContainer MongoDbContainer { get; private set; }

        public async Task UpAndRunningContainersAsync()
        {
            if (configuration == null)
            {
                return;
            }

            MongoDbContainer = new MongoDbBuilder()
                .WithImage("mongo:7.0.5-rc0-jammy")
                .WithName($"{nameof(TestContainersService)}-mongo-db-container")
                .WithHostname("test-mongo-db-host")
                .WithPortBinding(28117, 28117)
                .WithAutoRemove(true)
                .WithCleanUp(true)
                .Build();

            if (MongoDbContainer != null)
            {
                await MongoDbContainer.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            await MongoDbContainer.StopAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (MongoDbContainer != null)
            {
                await MongoDbContainer.DisposeAsync();
            }
        }
    }
}
