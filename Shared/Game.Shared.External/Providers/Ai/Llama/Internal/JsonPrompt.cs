namespace Game.Shared.External.Providers.Ai.Llama.Internal
{
    /// <summary>
    /// The JSON prompt we're sending to the Llama AI.
    /// </summary>
    internal class JsonPrompt
    {
        public string Prompt { get; set; } = string.Empty;
        public int N_predict { get; set; }
        public string[] Stop { get; set; } = Array.Empty<string>();
        public bool Stream { get; set; } = false;
    }
}
