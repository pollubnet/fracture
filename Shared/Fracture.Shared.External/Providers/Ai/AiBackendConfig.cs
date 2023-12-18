namespace Fracture.Shared.External.Providers.Ai
{
    /// <summary>
    /// The config for the AI provider's endpoint.
    /// </summary>
    public class AiBackendConfig
    {
        /// <summary>
        /// The endpoint's URL.
        /// </summary>
        public string EndpointUrl { get; set; } = null!;

        /// <summary>
        /// The API key to be used during the communication with the AI backend.
        /// </summary>
        public string? ApiKey { get; set; } = null;
    }
}
