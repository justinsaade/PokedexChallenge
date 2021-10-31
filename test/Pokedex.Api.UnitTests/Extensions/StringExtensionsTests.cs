using FluentAssertions;
using Pokedex.Api.Extensions;
using Xunit;

namespace Pokedex.Api.UnitTests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ReplaceEscapeSequenceWithSpaces_GivenStringWithEscapeSequences_ShouldReplaceWithSpaces()
        {
            // Arrange
            const string input = "A Pokémon created by recombining\nMew’s genes. It’s said to have the\nmost savage heart among Pokémon.";
            const string expected = "A Pokémon created by recombining Mew’s genes. It’s said to have the most savage heart among Pokémon.";

            // Act
            var result = input.ReplaceEscapeSequenceWithSpaces();

            // Assert
            result.Should().Be(expected);
        }
    }
}
