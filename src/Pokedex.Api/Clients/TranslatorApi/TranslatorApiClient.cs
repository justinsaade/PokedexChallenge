using Pokedex.Api.Clients.TranslatorApi.Models;
using Pokedex.Api.Exceptions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Api.Clients.TranslatorApi
{
    public class TranslatorApiClient : ITranslatorApiClient
    {
        private const string TranslationErrorMessage = "Unable to translate description.";

        private readonly HttpClient httpClient;

        public TranslatorApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TranslationResponse> TranslateYodaStyle(string description)
        {
            var result = await httpClient.PostAsJsonAsync("/translate/yoda.json", new TranslationRequest()
            {
                Text = description,
            });

            if (!result.IsSuccessStatusCode)
                throw new ExternalApiException(TranslationErrorMessage, result.StatusCode);

            return JsonSerializer.Deserialize<TranslationResponse>(await result.Content.ReadAsStringAsync());
        }

        public async Task<TranslationResponse> TranslateShakespearanStyle(string description)
        {
            var result = await httpClient.PostAsJsonAsync("/translate/shakespeare.json", new TranslationRequest()
            {
                Text = description,
            });

            if (!result.IsSuccessStatusCode)
                throw new ExternalApiException(TranslationErrorMessage, result.StatusCode);

            return JsonSerializer.Deserialize<TranslationResponse>(await result.Content.ReadAsStringAsync());
        }
    }
}