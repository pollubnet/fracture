using System.Text.Json.Serialization;

namespace Game.Shared.External.Providers.Ai.Llama.Internal
{
    /// <summary>
    /// The result of calling the Llama endpoint.
    /// </summary>
    internal class ChatResult
    {
        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Prompt { get; set; } = null!;
        public bool Stop { get; set; }
        public bool Stopped_eos { get; set; }
        public bool Stopped_limit { get; set; }
        public bool Stopped_word { get; set; }
        public string Stopping_word { get; set; } = null!;

        public int Tokens_cached { get; set; }
        public int Tokens_evaluated { get; set; }
        public int Tokens_predicted { get; set; }
        public bool Truncated { get; set; }
    }
}
