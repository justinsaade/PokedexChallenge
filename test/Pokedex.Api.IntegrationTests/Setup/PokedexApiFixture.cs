using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Pokedex.Api.IntegrationTests.Setup
{
    public class PokedexApiFixture : IDisposable
    {
        public const string PokedexApiTestHost = "http://localhost:60000";

        public TestServer PokedexApi { get; private set; }

        public PokedexApiFixture()
        {
            var webHostBuilder = new WebHostBuilder()
                        .UseEnvironment("Test")
                        .UseStartup<Startup>()
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            config.SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.Test.json");
                        })
                        .UseUrls(PokedexApiTestHost);

            PokedexApi = new TestServer(webHostBuilder);
        }

        public void Dispose()
        {
            PokedexApi.Dispose();
        }
    }
}