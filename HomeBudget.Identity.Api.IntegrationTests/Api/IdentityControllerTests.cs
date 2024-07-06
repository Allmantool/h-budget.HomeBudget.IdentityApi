using System;
using System.Threading.Tasks;

using FluentAssertions;
using RestSharp;

using HomeBudget.Core.Models;
using HomeBudget.Identity.Api.Constants;
using HomeBudget.Identity.Api.IntegrationTests.Constants;
using HomeBudget.Identity.Api.IntegrationTests.WebApps;
using HomeBudget.Identity.Api.Models;

namespace HomeBudget.Identity.Api.IntegrationTests.Api
{
    [Category(TestTypes.Integration)]
    [TestFixture]
    public class IdentityControllerTests : IAsyncDisposable
    {
        private const string ApiHost = $"api/{Endpoints.Identities}";

        private readonly IdentityTestWebApp _sut = new();

        [Test]
        public async Task Register_When_Then()
        {
            var requestBody = new RegisterRequest
            {
                Email = "test@email.com",
                Password = "secretPassword"
            };

            var registerRequest = new RestRequest($"{ApiHost}/register", Method.Post).AddJsonBody(requestBody);

            var response = await _sut.RestHttpClient.ExecuteAsync<Result<Guid>>(registerRequest);

            var result = response.Data;
            var payload = result.Payload;

            Assert.Multiple(() =>
            {
                response.IsSuccessful.Should().BeTrue();
            });
        }

        public async ValueTask DisposeAsync()
        {
            if (_sut != null)
            {
                await _sut.DisposeAsync();
            }
        }
    }
}
