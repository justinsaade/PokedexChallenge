using Pokedex.Api.Clients.PokeApi.Models;
using System.Threading.Tasks;

namespace Pokedex.Api.Clients.PokeApi
{
    public interface IPokeApiClient
    {
        public Task<PokemonSpecies> GetPokemon(string name);
    }
}