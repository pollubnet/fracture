using System.Text.Json;
using Game.Shared.External.Providers.Ai.Llama.Internal;
using Microsoft.Extensions.Options;

namespace Game.Shared.External.Providers.Ai.Llama
{
    /// <summary>
    /// Provides access to the Llama AI model.
    /// </summary>
    public class LlamaAiProvider : IAiProvider
    {
        /// <summary>
        /// The client used to communicate with the AI provider.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Constructs a new Llama provider.
        /// </summary>
        public LlamaAiProvider(IOptions<AiEndpointConfig> opts)
        {
            _client = new HttpClient { BaseAddress = new(opts.Value.EndpointUrl) };
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
                N_predict = context.TokenCount,
                Stop = context.StopTokens,
            };

            return JsonSerializer.Serialize(jsonPrompt);
        }

        /// <inheritdoc/>
        public async Task<string> Generate(AiGenerationContext context)
        {
            var request = new HttpRequestMessage
            {
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
