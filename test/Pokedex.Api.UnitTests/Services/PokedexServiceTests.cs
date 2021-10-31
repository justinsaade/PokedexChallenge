using AutoFixture;
using FluentAssertions;
using Moq;
using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Clients.PokeApi.Models;
using Pokedex.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.UnitTests.Services
{
    public class PokedexServiceTests
    {
        private readonly Mock<IPokeApiClient> pokeApiClient;
        private readonly IPokedexService pokedexService;
        private readonly Fixture fixture;

        public PokedexServiceTests()
        {
            pokeApiClient = new Mock<IPokeApiClient>();
            pokedexService = new PokedexService(pokeApiClient.Object);
            fixture = new Fixture();
        }

        [Fact]
        public async Task GetPokemonDetails_GivenPokeApiClientReturnsData_ShouldUseDescription_FromFirstEnglishDescription()
        {
            // Arrange
            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.FlavorTextEntries, new List<FlavorTextEntry>()
                {
                    new FlavorTextEntry()
                    {
                        FlavorText = "Some string",
                        Language = new Language() { Name = "en" },
                        Version = new Version(),
                    },
                    new FlavorTextEntry()
                    {
                        FlavorText = "Some other string",
                        Language = new Language() { Name = "en" },
                        Version = new Version(),
                    },
                }).Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            // Act
            var result = await pokedexService.GetPokemonDetails("whateverString");

            // Assert
            result.Description.Should().Be(pokemonSpecies.FlavorTextEntries[0].FlavorText);
        }
    }
}
