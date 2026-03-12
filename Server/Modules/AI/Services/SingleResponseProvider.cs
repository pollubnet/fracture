using Fracture.Server.Modules.AI.Models;
using Microsoft.Extensions.AI;

namespace Fracture.Server.Modules.AI.Services;

public class SingleResponseProvider(IChatClient client)
{
    private readonly IChatClient _client = client;

    /// <summary>
    /// Returns a response generated for a given prompt.
    /// </summary>
    /// <param name="prompt">A simple prompt</param>
    /// <returns>A LLM response as a string</returns>
    public async Task<string?> GenerateResponse(string prompt)
    {
        return await GenerateResponse(new AIGenerationContext { Prompt = prompt });
    }

    /// <summary>
    /// Returns a response generated for a given generation context, which may include both prompt and additional parameters such as temperature.
    /// </summary>
    /// <param name="context">A full generation context</param>
    /// <returns>A LLM response as a string</returns>
    public async Task<string?> GenerateResponse(AIGenerationContext context)
    {
        var messages = new List<ChatMessage> { new(ChatRole.User, context.Prompt) };

        var options = new ChatOptions
        {
            TopP = context.TopP,
            Temperature = context.Temperature,
            MaxOutputTokens = context.MaxTokens,
            StopSequences = context.StopTokens,
            ModelId = context.Model,
        };

        var response = await _client.GetResponseAsync(messages, options);

        return response.Text;
    }
}
