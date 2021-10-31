using Xunit;

namespace Pokedex.Api.IntegrationTests.Setup
{
    [CollectionDefinition("PokedexApi")]
    public class PokedexApiCollection : ICollectionFixture<PokedexApiFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}