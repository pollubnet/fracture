using System.Text.Json;
using Fracture.Shared.External.Providers.Ai;
using Fracture.Shared.External.Providers.Ai.Llama.Internal;
using Fracture.Shared.External.Providers.Ai.LlamaCpp.Internal;
using Microsoft.Extensions.Options;

namespace Fracture.Shared.External.Providers.Ai.LlamaCpp
{
    /// <summary>
    /// Provides access to the Llama AI model.
    /// </summary>
    public class LlamaCppBackendProvider : IAiBackendProvider
    {
        /// <summary>
        /// The client used to communicate with the AI provider.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Constructs a new Llama provider.
        /// </summary>
        public LlamaCppBackendProvider(IOptions<AiBackendConfig> opts)
        {
            _client = new HttpClient { BaseAddress = new(opts.Value.EndpointUrl) };

            if (opts.Value.ApiKey is not null)
            {
                _client.DefaultRequestHeaders.Add("X-ApiKey", opts.Value.ApiKey);
            }
        }

        /// <summary>
        /// Generates a JSON prompt to send to the endpoint from the given context.
        /// </summary>
        /// <param name="context">The generation context.</param>
        /// <returns>The JSON prompt.</returns>
        private string GenerateJSONPrompt(AiGenerationContext context)
        {
            var jsonPrompt = new JsonPrompt
            {
                Prompt = context.Prompt,
                MaxTokens = context.MaxTokens,
                Stop = context.StopTokens,
                Temperature = context.Temperature,
                TopP = context.TopP,
            };

            return JsonSerializer.Serialize(jsonPrompt);
        }

        /// <inheritdoc/>
        public async Task<string> Generate(AiGenerationContext context)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(GenerateJSONPrompt(context))
            };

            var resp = await _client.SendAsync(request);
            resp.EnsureSuccessStatusCode();

            var body = await resp.Content.ReadAsStringAsync();

            // NOTE: If we receive a success code, this must be a valid chat result, no?
            var chatResult = JsonSerializer.Deserialize<ChatResult>(body)!;
            return chatResult.Content.Trim();
        }
    }
}
