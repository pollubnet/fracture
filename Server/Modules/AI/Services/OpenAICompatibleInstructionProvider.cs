using Fracture.Server.Modules.AI.Models;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace Fracture.Server.Modules.AI.Services;

public class OpenAICompatibleInstructionProvider : IAIInstructionProvider
{
    private readonly OpenAIClient _api;
    private readonly AIBackendConfiguration _configuration;

    public OpenAICompatibleInstructionProvider(IOptions<AIBackendConfiguration> configuration)
    {
        _configuration = configuration.Value;
        if (_configuration.ApiKey is null)
        {
            ArgumentException.ThrowIfNullOrEmpty(_configuration.ApiKey);
        }

        if (_configuration.EndpointUrl is null)
        {
            _api = new OpenAIClient(new OpenAIAuthentication(_configuration.ApiKey));
        }
        else
        {
            var settings = new OpenAIClientSettings(domain: _configuration.EndpointUrl);

            _api = new OpenAIClient(
                new OpenAIAuthentication(_configuration.ApiKey),
                clientSettings: settings
            );
        }
    }

    public async Task<string> GenerateInstructionResponse(string instruction)
    {
        return await GenerateResponse(new() { Prompt = instruction });
    }

    public async Task<string> GenerateResponse(AIGenerationContext context)
    {
        var messages = new List<Message> { new Message(Role.User, context.Prompt) };

        var chatRequest = new ChatRequest(
            messages,
            model: context.Model ?? _configuration.Model,
            temperature: context.Temperature,
            stops: context.StopTokens,
            maxTokens: context.MaxTokens,
            topP: context.TopP
        );
        var response = await _api.ChatEndpoint.GetCompletionAsync(chatRequest);

        return response.FirstChoice;
    }
}
