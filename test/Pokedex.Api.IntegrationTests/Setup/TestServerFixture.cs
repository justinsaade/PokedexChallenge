using System;
using System.IO;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace Pokedex.Api.IntegrationTests.Setup
{
    public class TestServerFixture : IDisposable
    {
        private const string PokeApiUrl = "http://+:59000";

        private readonly WireMockServer server;

        public TestServerFixture()
        {
            server = WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] { PokeApiUrl }
            });
        }

        public void Dispose()
        {
            server.Stop();
        }

        public void Reset()
        {
            server.Reset();
        }

        public IRequestBuilder SetupGetPokemonSpecies(string responseBodyResource, int statusCode = 200)
        {
            var request = Request.Create()
                .UsingGet()
                .WithPath("/api/v2/pokemon-species/mewtwo");

            var responseBody = string.IsNullOrWhiteSpace(responseBodyResource) ? Array.Empty<byte>() : File.ReadAllBytes(responseBodyResource);

            server.Given(request)
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("content-type", "application/json")
                    .WithBody(responseBody)
                );

            return request;
        }

        public IRequestBuilder SetupTranslation(string responseBodyResource, int statusCode = 200)
        {
            var request = Request.Create()
                .UsingPost()
                .WithPath("/translate/yoda.json", "/translate/shakespeare.json");

            var responseBody = string.IsNullOrWhiteSpace(responseBodyResource) ? Array.Empty<byte>() : File.ReadAllBytes(responseBodyResource);

            server.Given(request)
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(statusCode)
                    .WithHeader("content-type", "application/json")
                    .WithBody(responseBody)
                );

            return request;
        }
    }
}