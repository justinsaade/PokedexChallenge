using System.Text.Json.Serialization;

namespace Pokedex.Api.Clients.PokeApi.Models
{
    public class EvolutionChain
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
