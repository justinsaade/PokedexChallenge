using Pokedex.Api.Clients.TranslatorApi.Models;
using System.Threading.Tasks;

namespace Pokedex.Api.Clients.TranslatorApi
{
    public interface ITranslatorApiClient
    {
        public Task<TranslationResponse> TranslateYodaStyle(string description);

        public Task<TranslationResponse> TranslateShakespearanStyle(string description);
    }
}
