using Pokedex.Api.Clients.PokeApi.Models;
using System.Net.Http;
using System.Net.Http.Json;
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
            return await httpClient.GetFromJsonAsync<PokemonSpecies>($"/api/v2/pokemon-species/{name}");
        }
    }
}
