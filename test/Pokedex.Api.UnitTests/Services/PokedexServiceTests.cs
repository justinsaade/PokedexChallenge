using AutoFixture;
using FluentAssertions;
using Moq;
using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Clients.PokeApi.Models;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Clients.TranslatorApi.Models;
using Pokedex.Api.Exceptions;
using Pokedex.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.UnitTests.Services
{
    // TO:DO Refactor the redudant code using ICustomization classes for the Fixtures
    public class PokedexServiceTests
    {
        private readonly Mock<IPokeApiClient> pokeApiClient;
        private readonly Mock<ITranslatorApiClient> translatorApiClient;
        private readonly IPokedexService pokedexService;
        private readonly Fixture fixture;

        public PokedexServiceTests()
        {
            pokeApiClient = new Mock<IPokeApiClient>();
            translatorApiClient = new Mock<ITranslatorApiClient>();
            pokedexService = new PokedexService(pokeApiClient.Object, translatorApiClient.Object);
            fixture = new Fixture();
        }

        [Fact]
        public async Task GetPokemonDetails_GivenPokeApiClientReturnsData_ShouldUseDescription_FromFirstEnglishDescription()
        {
            // Arrange
            const string name = "whateverString";

            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.Name, name)
                .With(p => p.IsLegendary, false)
                .With(p => p.FlavorTextEntries, MockDescription())
                .Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            // Act
            var result = await pokedexService.GetPokemonDetails(name);

            // Assert
            result.Description.Should().Be(pokemonSpecies.FlavorTextEntries[0].FlavorText);
        }

        [Fact]
        public async Task GetPokemonDetails_GivenPokeApiClientReturnsData_WithHabitatAsCave_ShouldRequestYodaTranslationOfDescription()
        {
            // Arrange
            const string name = "whateverString";

            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.Name, name)
                .With(p => p.IsLegendary, true)
                .With(p => p.FlavorTextEntries, MockDescription())
                .With(p => p.Habitat, new Habitat() { Name = "Cave" }).Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            translatorApiClient.Setup(x => x.TranslateYodaStyle(It.IsAny<string>()))
                .ReturnsAsync(new TranslationResponse() { Contents = new ContentsModel() { Translated = "someTranslation" } });

            // Act
            await pokedexService.GetPokemonDetails(name, true);

            // Assert
            translatorApiClient.Verify(
                x => x.TranslateYodaStyle(It.Is<string>(x => x == pokemonSpecies.FlavorTextEntries[0].FlavorText)),
                Times.Once());
        }

        [Fact]
        public async Task GetPokemonDetails_GivenPokeApiClientReturnsData_WithHabitatAsLegendary_ShouldRequestYodaTranslationOfDescription()
        {
            // Arrange
            const string name = "whateverString";

            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.Name, name)
                .With(p => p.IsLegendary, true)
                .With(p => p.FlavorTextEntries, MockDescription())
                .With(p => p.Habitat, new Habitat() { Name = "Legendary" }).Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            translatorApiClient.Setup(x => x.TranslateYodaStyle(It.IsAny<string>()))
                .ReturnsAsync(new TranslationResponse() { Contents = new ContentsModel() { Translated = "someTranslation" } });

            // Act
            await pokedexService.GetPokemonDetails(name, true);

            // Assert
            translatorApiClient.Verify(
                x => x.TranslateYodaStyle(It.Is<string>(x => x == pokemonSpecies.FlavorTextEntries[0].FlavorText)),
                Times.Once());
        }

        [Fact]
        public async Task GetPokemonDetails_GivenPokeApiClientReturnsData_WithNonCaveOrLegendary_ShouldRequestShakespeareanTranslationOfDescription()
        {
            // Arrange
            const string name = "whateverString";

            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.Name, name)
                .With(p => p.IsLegendary, true)
                .With(p => p.FlavorTextEntries, MockDescription())
                .With(p => p.Habitat, new Habitat() { Name = "Water" }).Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            translatorApiClient.Setup(x => x.TranslateShakespearanStyle(It.IsAny<string>()))
                .ReturnsAsync(new TranslationResponse() { Contents = new ContentsModel() { Translated = "someTranslation" } });

            // Act
            await pokedexService.GetPokemonDetails(name, true);

            // Assert
            translatorApiClient.Verify(
                x => x.TranslateShakespearanStyle(It.Is<string>(x => x == pokemonSpecies.FlavorTextEntries[0].FlavorText)),
                Times.Once());
        }

        [Fact]
        public async Task GetPokemonDetails_GivenTranslatorApi_TranslateYodaStyle_ThrowsAnException_UseDefaultDescription()
        {
            // Arrange
            const string name = "whateverString";

            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.Name, name)
                .With(p => p.IsLegendary, true)
                .With(p => p.FlavorTextEntries, MockDescription())
                .With(p => p.Habitat, new Habitat() { Name = "Legendary" }).Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            translatorApiClient.Setup(x => x.TranslateYodaStyle(It.IsAny<string>()))
                .ThrowsAsync(new ExternalApiException());

            // Act
            var result = await pokedexService.GetPokemonDetails(name, true);

            // Assert
            translatorApiClient.Verify(
                x => x.TranslateYodaStyle(It.Is<string>(x => x == pokemonSpecies.FlavorTextEntries[0].FlavorText)),
                Times.Once());

            result.Description.Should().Be(pokemonSpecies.FlavorTextEntries[0].FlavorText);
        }

        [Fact]
        public async Task GetPokemonDetails_GivenTranslatorApi_TranslateShakespearanStyle_ThrowsAnException_UseDefaultDescription()
        {
            // Arrange
            const string name = "whateverString";

            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.Name, name)
                .With(p => p.IsLegendary, true)
                .With(p => p.FlavorTextEntries, MockDescription())
                .With(p => p.Habitat, new Habitat() { Name = "Water" }).Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            translatorApiClient.Setup(x => x.TranslateShakespearanStyle(It.IsAny<string>()))
                .ThrowsAsync(new ExternalApiException());

            // Act
            var result = await pokedexService.GetPokemonDetails(name, true);

            // Assert
            translatorApiClient.Verify(
                x => x.TranslateShakespearanStyle(It.Is<string>(x => x == pokemonSpecies.FlavorTextEntries[0].FlavorText)),
                Times.Once());

            result.Description.Should().Be(pokemonSpecies.FlavorTextEntries[0].FlavorText);
        }

        [Fact]
        public async Task GetPokemonDetails_GivenPokeApi_ReturnsAnInvalidFieldThrowsException()
        {
            // Arrange
            const string name = "whateverString";

            var pokemonSpecies = fixture.Build<PokemonSpecies>()
                .With(p => p.Name, "NottheCorrectName")
                .With(p => p.IsLegendary, true)
                .With(p => p.FlavorTextEntries, MockDescription())
                .With(p => p.Habitat, new Habitat() { Name = "Water" }).Create();

            pokeApiClient.Setup(x => x.GetPokemon(It.IsAny<string>())).ReturnsAsync(pokemonSpecies);

            translatorApiClient.Setup(x => x.TranslateShakespearanStyle(It.IsAny<string>()))
                .ThrowsAsync(new ExternalApiException());

            // Act
            Func<Task> act = async () => await pokedexService.GetPokemonDetails(name, true);

            // Assert
            await act.Should().ThrowAsync<PokemonDetailsRetrievalException>();
        }

        private List<FlavorTextEntry> MockDescription()
        {
            return new List<FlavorTextEntry>()
            {
                new FlavorTextEntry()
                {
                    FlavorText = "Some string",
                    Language = new Language() { Name = "en" },
                    Version = new Api.Clients.PokeApi.Models.Version(),
                },
            };
        }
    }
}