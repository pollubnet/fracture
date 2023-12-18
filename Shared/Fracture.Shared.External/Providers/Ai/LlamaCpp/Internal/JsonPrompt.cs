using System.Text.Json.Serialization;

namespace Fracture.Shared.External.Providers.Ai.LlamaCpp.Internal
{
    /// <summary>
    /// The JSON prompt we're sending to the Llama AI.
    /// </summary>
    internal class JsonPrompt
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("n_predict")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("stop")]
        public string[] Stop { get; set; } = Array.Empty<string>();

        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false;

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.7;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 0.5;
    }
}
