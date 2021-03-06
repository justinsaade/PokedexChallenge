using System.Text.Json.Serialization;

namespace Pokedex.Api.ViewModels
{
    public class PokemonViewModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("habitat")]
        public string Habitat { get; set; }

        [JsonPropertyName("isLegendary")]
        public bool IsLegendary { get; set; }
    }
}