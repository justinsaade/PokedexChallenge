using System.Text.Json.Serialization;

namespace Pokedex.Api.Clients.PokeApi.Models
{
    public class Genera
    {
        [JsonPropertyName("genus")]
        public string Genus { get; set; }

        [JsonPropertyName("language")]
        public Language Language { get; set; }
    }
}