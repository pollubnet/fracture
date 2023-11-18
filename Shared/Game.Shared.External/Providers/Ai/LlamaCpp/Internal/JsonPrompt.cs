using System.Text.Json.Serialization;

namespace Game.Shared.External.Providers.Ai.LlamaCpp.Internal
{
    /// <summary>
    /// The JSON prompt we're sending to the Llama AI.
    /// </summary>
    internal class JsonPrompt
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("n_predict")]
        public int NumberPredicted { get; set; }

        [JsonPropertyName("stop")]
        public string[] Stop { get; set; } = Array.Empty<string>();

        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false;
    }
}
