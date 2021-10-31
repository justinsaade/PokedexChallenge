using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.Api.Services
{
    public class PokedexService : IPokedexService
    {
        private const string DefaultLanguage = "en";

        private readonly IPokeApiClient pokeApiClient;

        public PokedexService(IPokeApiClient pokeApiClient)
        {
            this.pokeApiClient = pokeApiClient;
        }

        public async Task<PokemonViewModel> GetPokemonDetails(string name)
        {
            var pokemonSpecies = await this.pokeApiClient.GetPokemon(name);

            var pokemonDescription = pokemonSpecies.FlavorTextEntries.FirstOrDefault(x => x.Language.Name == DefaultLanguage)?.FlavorText;

            return new PokemonViewModel()
            {
                Name = pokemonSpecies.Name,
                IsLegendary = pokemonSpecies.IsLegendary,
                Description = pokemonDescription,
                Habitat = pokemonSpecies.Habitat.Name,
            };
        }

        public async Task<PokemonViewModel> GetTranslatedPokemonDetails(string name)
        {
            throw new NotImplementedException();
        }
    }
}
