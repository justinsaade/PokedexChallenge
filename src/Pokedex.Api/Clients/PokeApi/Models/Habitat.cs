﻿using System.Text.Json.Serialization;

namespace Pokedex.Api.Clients.PokeApi.Models
{
    public class Habitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}