using FluentAssertions;
using Pokedex.Api.IntegrationTests.Setup;
using Pokedex.Api.ViewModels;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.IntegrationTests
{
    [Collection("PokedexApi")]
    public class PokemonControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture testServer;
        private readonly PokedexApiFixture pokedexApiFixture;

        public PokemonControllerTests(TestServerFixture testServer, PokedexApiFixture pokedexApiFixture)
        {
            this.testServer = testServer;
            this.pokedexApiFixture = pokedexApiFixture;
        }

        [Fact]
        public async Task Given_PokeApi_ReturnsSuccess_Returns_PokemonDetails()
        {
            // Arrange
            testServer.SetupGetPokemonSpecies("Resources/success.json");

            using (var client = pokedexApiFixture.PokedexApi.CreateClient())
            {
                // Act
                var result = await client.GetAsync("http://localhost:60000/Pokemon/mewtwo");

                var json = await result.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<PokemonViewModel>(json);

                // Assert
                result.StatusCode.Should().Be(HttpStatusCode.OK);

                response.Name.Should().Be("mewtwo");
                response.Habitat.Should().Be("rare");
                response.Description.Should().Be("It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.");
                response.IsLegendary.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Given_PokeApi_Returns404_Returns_HandlesTheErrorAndReturns404()
        {
            // Arrange
            testServer.SetupGetPokemonSpecies(null, 404);

            using (var client = pokedexApiFixture.PokedexApi.CreateClient())
            {
                // Act
                var result = await client.GetAsync("http://localhost:60000/Pokemon/notavalidpokemon");

                var json = await result.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<PokemonViewModel>(json);

                // Assert
                result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
