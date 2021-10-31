using System.Text.Json.Serialization;

namespace Pokedex.Api.Clients.PokeApi.Models
{
    public class Names
    {
        [JsonPropertyName("language")]
        public Language Language { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}