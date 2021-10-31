using System.Text.Json.Serialization;

namespace Pokedex.Api.Clients.PokeApi.Models
{
    public class PalParkEncounter
    {
        [JsonPropertyName("area")]
        public Area Area { get; set; }

        [JsonPropertyName("base_score")]
        public int BaseScore { get; set; }

        [JsonPropertyName("rate")]
        public int Rate { get; set; }
    }
}