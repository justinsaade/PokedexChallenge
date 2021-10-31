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
        private const string PokemonSpeciesSucessPath = "Resources/PokeApi/success.json";
        private const string TranslationSuccessPath = "Resources/TranslatorApi/success.json";

        private const string PokemonSpeciesSuccessDescription = "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.";
        private const string PokemonSpeciesSuccessName = "mewtwo";
        private const string PokemonSpeciesSuccessHabitat = "rare";

        private const string TranslationSuccessResult = "Lost a planet,  master obiwan has.";

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
            testServer.Reset();
            testServer.SetupGetPokemonSpecies(PokemonSpeciesSucessPath);

            using var client = pokedexApiFixture.PokedexApi.CreateClient();

            // Act
            var result = await client.GetAsync("http://localhost:60000/Pokemon/mewtwo");

            var json = await result.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<PokemonViewModel>(json);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            response.Name.Should().Be(PokemonSpeciesSuccessName);
            response.Habitat.Should().Be(PokemonSpeciesSuccessHabitat);
            response.Description.Should().Be(PokemonSpeciesSuccessDescription);
            response.IsLegendary.Should().BeTrue();
        }

        [Fact]
        public async Task Given_PokeApi_Returns404_Returns_HandlesTheErrorAndReturns404()
        {
            // Arrange
            testServer.Reset();
            testServer.SetupGetPokemonSpecies(null, (int)HttpStatusCode.NotFound);

            using var client = pokedexApiFixture.PokedexApi.CreateClient();

            // Act
            var result = await client.GetAsync("http://localhost:60000/Pokemon/notavalidpokemon");

            var json = await result.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<PokemonViewModel>(json);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Given_TranslatorApi_ReturnsSuccess_ShouldRequestTranslationForPokemonDescription()
        {
            // Arrange
            testServer.Reset();

            testServer.SetupGetPokemonSpecies(PokemonSpeciesSucessPath);
            testServer.SetupTranslation(TranslationSuccessPath);

            using var client = pokedexApiFixture.PokedexApi.CreateClient();

            // Act
            var result = await client.GetAsync("http://localhost:60000/Pokemon/translated/mewtwo");

            var json = await result.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<PokemonViewModel>(json);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            response.Name.Should().Be(PokemonSpeciesSuccessName);
            response.Habitat.Should().Be(PokemonSpeciesSuccessHabitat);
            response.Description.Should().Be(TranslationSuccessResult);
            response.IsLegendary.Should().BeTrue();
        }

        [Fact]
        public async Task Given_TranslatorApi_ReturnsTooManyRequests_ShouldReturnsOriginalDescription()
        {
            // Arrange
            testServer.Reset();

            testServer.SetupGetPokemonSpecies(PokemonSpeciesSucessPath);
            testServer.SetupTranslation(TranslationSuccessPath, (int)HttpStatusCode.TooManyRequests);

            using var client = pokedexApiFixture.PokedexApi.CreateClient();

            // Act
            var result = await client.GetAsync("http://localhost:60000/Pokemon/translated/mewtwo");

            var json = await result.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<PokemonViewModel>(json);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            response.Name.Should().Be(PokemonSpeciesSuccessName);
            response.Habitat.Should().Be(PokemonSpeciesSuccessHabitat);
            response.Description.Should().Be(PokemonSpeciesSuccessDescription);
            response.IsLegendary.Should().BeTrue();
        }
    }
}
