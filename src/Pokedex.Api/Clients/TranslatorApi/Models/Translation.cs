using System.Text.Json.Serialization;

namespace Pokedex.Api.Clients.TranslatorApi.Models
{
    public class TranslationRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class TranslationResponse
    {
        [JsonPropertyName("success")]
        public SuccessModel Success { get; set; }

        [JsonPropertyName("contents")]
        public ContentsModel Contents { get; set; }
    }

    public class SuccessModel
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class ContentsModel
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("translation")]
        public string Translation { get; set; }
    }
}