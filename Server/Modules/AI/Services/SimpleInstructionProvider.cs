using Fracture.Server.Modules.AI.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;

namespace Fracture.Server.Modules.AI.Services;

public class SimpleInstructionProvider(IChatClient client) : IAIInstructionProvider
{
    private readonly IChatClient _client = client;

    public async Task<string?> GenerateInstructionResponse(string instruction)
    {
        return await GenerateResponse(new AIGenerationContext { Prompt = instruction });
    }

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

        return response
            .Messages.FirstOrDefault()
            ?.Contents?.FirstOrDefault()
            ?.RawRepresentation?.ToString();
    }
}
