namespace Fracture.Server.Modules.AI.Models;

/// <summary>
///     The config for the AI provider's endpoint.
/// </summary>
public class AIBackendConfiguration
{
    /// <summary>
    ///     The OpenAI-compatible endpoint's URL. If null, the default one (OpenAI)
    ///     will be used.
    /// </summary>
    public string? EndpointUrl { get; set; }

    /// <summary>
    ///     The API key to be used during the communication with the AI backend.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    ///     Name of the model to be used (e.g. "chatgpt-3.5-turbo" or "mistral")
    ///     Some servers (e.g. llama.cpp) may ignore that.
    /// </summary>
    public required string Model { get; set; }
}
