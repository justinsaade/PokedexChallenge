using Pokedex.Api.Clients.PokeApi.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pokedex.Api.Clients.PokeApi
{
    public class PokeApiClient : IPokeApiClient
    {
        private HttpClient httpClient;

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
