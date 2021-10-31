using System.Text.Json.Serialization;

namespace Pokedex.Api.Clients.PokeApi.Models
{
    public class PokedexNumber
    {
        [JsonPropertyName("entry_number")]
        public int EntryNumber { get; set; }

        [JsonPropertyName("pokedex")]
        public Pokedex Pokedex { get; set; }
    }
}
