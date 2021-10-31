using Pokedex.Api.Clients.PokeApi.Models;
using Pokedex.Api.Exceptions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Api.Clients.PokeApi
{
    /*
     * TO:DO Implement exponential backoff policy or circuit breaker pattern using Polly.
     * To handle transient faults and long lasting transient faults respectively e.g. for the rate limiter.
     */

    public class PokeApiClient : IPokeApiClient
    {
        private readonly HttpClient httpClient;

        public PokeApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PokemonSpecies> GetPokemon(string name)
        {
            var result = await httpClient.GetAsync($"/api/v2/pokemon-species/{name}");

            if (!result.IsSuccessStatusCode)
                throw new ExternalApiException("Unable to retreieve pokemon details.", result.StatusCode);

            return JsonSerializer.Deserialize<PokemonSpecies>(await result.Content.ReadAsStringAsync());
        }
    }
}