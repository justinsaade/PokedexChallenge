using Pokedex.Api.Clients.TranslatorApi.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Api.Clients.TranslatorApi
{
    public class TranslatorApiClient : ITranslatorApiClient
    {
        private readonly HttpClient httpClient;

        public TranslatorApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TranslationResponse> TranslateYodaStyle(string description)
        {
            var response = await httpClient.PostAsJsonAsync("/translate/yoda.json", new TranslationRequest()
            {
                Text = description,
            });

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TranslationResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task<TranslationResponse> TranslateShakespearanStyle(string description)
        {
            var response = await httpClient.PostAsJsonAsync("/translate/shakespeare.json", new TranslationRequest()
            {
                Text = description,
            });

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TranslationResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
