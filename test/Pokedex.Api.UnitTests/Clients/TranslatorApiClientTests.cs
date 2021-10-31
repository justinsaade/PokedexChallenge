using FluentAssertions;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.UnitTests.Clients
{
    public class TranslatorApiClientTests
    {
        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        public async Task TranslateYodaStyle_ThrowsExternalApiException_IfNonSuccess(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpClient = HttpClientTestHelper.SetupHttpClient(httpStatusCode);

            var subject = new TranslatorApiClient(mockHttpClient);

            // Act
            Func<Task> act = async () => await subject.TranslateYodaStyle("SomeName");

            // Assert
            await act.Should().ThrowAsync<ExternalApiException>();
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        public async Task TranslateShakespearanStylen_ThrowsExternalApiException_IfNonSuccess(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpClient = HttpClientTestHelper.SetupHttpClient(httpStatusCode);

            var subject = new TranslatorApiClient(mockHttpClient);

            // Act
            Func<Task> act = async () => await subject.TranslateShakespearanStyle("SomeName");

            // Assert
            await act.Should().ThrowAsync<ExternalApiException>();
        }
    }
}
