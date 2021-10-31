using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Exceptions;
using Xunit;

namespace Pokedex.Api.UnitTests.Clients
{
    public class PokeApiTests
    {
        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        public async Task GetPokemon_ThrowsExternalApiException_IfNonSuccess(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpClient = HttpClientTestHelper.SetupHttpClient(httpStatusCode);

            var subject = new PokeApiClient(mockHttpClient);

            // Act
            Func<Task> act = async () => await subject.GetPokemon("SomeName");

            // Assert
            await act.Should().ThrowAsync<ExternalApiException>();
        }
    }
}
