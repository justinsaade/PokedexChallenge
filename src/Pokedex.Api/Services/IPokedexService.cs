using Pokedex.Api.ViewModels;
using System.Threading.Tasks;

namespace Pokedex.Api.Services
{
    public interface IPokedexService
    {
        public Task<PokemonViewModel> GetPokemonDetails(string name, bool translate = false);
    }
}