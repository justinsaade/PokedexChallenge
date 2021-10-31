using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Clients.TranslatorApi.Models;
using Pokedex.Api.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.Api.Services
{
    // TO:DO Improve with a cache possibly here or the client itself.
    // TO:DO Introduce AutoMapper to remove mapping logic from here
    public class PokedexService : IPokedexService
    {
        private const string DefaultLanguage = "en";

        private readonly IPokeApiClient pokeApiClient;
        private readonly ITranslatorApiClient translatorApiClient;

        public PokedexService(IPokeApiClient pokeApiClient,
            ITranslatorApiClient translatorApiClient)
        {
            this.pokeApiClient = pokeApiClient;
            this.translatorApiClient = translatorApiClient;
        }

        public async Task<PokemonViewModel> GetPokemonDetails(string name, bool translate = false)
        {
            var pokemonSpecies = await pokeApiClient.GetPokemon(name.ToLowerInvariant());

            var pokemonDescription = pokemonSpecies.FlavorTextEntries
                .FirstOrDefault(x => x.Language.Name == DefaultLanguage)?.FlavorText;

            if (translate)
            {
                pokemonDescription = await GetTranslatedDescription(pokemonDescription, pokemonSpecies.Habitat.Name);
            }

            return new PokemonViewModel()
            {
                Name = pokemonSpecies.Name,
                IsLegendary = pokemonSpecies.IsLegendary,
                Description = pokemonDescription,
                Habitat = pokemonSpecies.Habitat.Name,
            };
        }

        private async Task<string> GetTranslatedDescription(string description, string habitat)
        {
            try
            {
                TranslationResponse translationResponse;

                if (HabitatIsCaveOrLegendary(habitat))
                {
                    translationResponse = await translatorApiClient.TranslateYodaStyle(description);
                }
                else
                {
                    translationResponse = await translatorApiClient.TranslateShakespearanStyle(description);
                }

                return translationResponse.Contents.Translated;

            }
            catch (Exception)
            {
                // TO:DO Log the failure so we can track how many times translation fails.
                // As per the specification this should fail silently i.e. return to the user 200 with default description.
                return description;
            }
        }

        private static bool HabitatIsCaveOrLegendary(string habitat)
        {
            return string.Equals(habitat, "cave", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(habitat, "legendary", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
