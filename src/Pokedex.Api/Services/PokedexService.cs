using Pokedex.Api.Clients.PokeApi;
using Pokedex.Api.Clients.PokeApi.Models;
using Pokedex.Api.Clients.TranslatorApi;
using Pokedex.Api.Clients.TranslatorApi.Models;
using Pokedex.Api.Exceptions;
using Pokedex.Api.Extensions;
using Pokedex.Api.ViewModels;
using System;
using System.Threading.Tasks;

namespace Pokedex.Api.Services
{
    // TO:DO Improve with a cache possibly here or the client itself.
    // TO:DO Introduce AutoMapper to remove mapping logic from here
    // TO:DO Refactor validations out of here possibly using FleuntValidations
    public class PokedexService : IPokedexService
    {
        private const string DefaultLanguage = "en";
        private const string NoDescriptionErrorMessage = "No description was retrieved to translate.";
        private const string IncorrectPokemonDetailsError = "Incorrect Pokemon details retrieved from PokeApi.";

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

            if (!HasRequiredProperties(pokemonSpecies, name))
                throw new PokemonDetailsRetrievalException(IncorrectPokemonDetailsError);

            var pokemonDescription = pokemonSpecies.FlavorTextEntries
                .Find(x => x.Language.Name.IsEqualTo(DefaultLanguage))?
                .FlavorText?
                .ReplaceEscapeSequenceWithSpaces();

            if (translate)
            {
                pokemonDescription = await GetTranslatedDescription(
                    pokemonDescription, pokemonSpecies.Habitat.Name);
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
            if (string.IsNullOrWhiteSpace(description)) throw new TranslationException(NoDescriptionErrorMessage);

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
            catch (Exception exception)
            {
                // TO:DO Log the failure so we can track how many times translation fails.
                // As per the specification this should return to the user with the default description.

                if (exception is ExternalApiException) return description;

                throw;
            }
        }

        private static bool HabitatIsCaveOrLegendary(string habitat)
        {
            return string.Equals(habitat, "cave", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(habitat, "legendary", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool HasRequiredProperties(PokemonSpecies pokemonSpecies, string name)
        {
            return pokemonSpecies.Name.IsEqualTo(name) &&
                pokemonSpecies?.IsLegendary != null
                && pokemonSpecies?.FlavorTextEntries?.Count > 0
                && pokemonSpecies?.Habitat?.Name != null;
        }
    }
}