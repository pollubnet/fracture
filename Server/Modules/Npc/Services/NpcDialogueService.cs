using Fracture.Server.Modules.Npc.Models;
using Microsoft.Extensions.AI;

namespace Fracture.Server.Modules.Npc.Services;

public class NpcDialogueService(IChatClient? chatClient = null)
{
    public async Task SendMessageAsync(NpcEntity npc, List<string> history)
    {
        if (chatClient is null)
            throw new InvalidOperationException("The chatbot is not available.");

        var systemMessage = $"""
            You are {npc.Name}, {npc.Role} a character in a fantasy role-playing game.

            Your history: {npc.History}

            Reply in character, naturally and concisely. Do not talk about the real world, talk only about the game world.
            """;

        var chatMessages = new List<ChatMessage> { new(ChatRole.System, systemMessage) };

        chatMessages.AddRange(
            history.Select(entry =>
            {
                var isUserMessage = entry.StartsWith("You: ");
                return new ChatMessage(
                    isUserMessage ? ChatRole.User : ChatRole.Assistant,
                    entry[5..]
                );
            })
        );
        var response = await chatClient.GetResponseAsync(chatMessages);
        var responseText = response.Text?.Trim();
        if (string.IsNullOrWhiteSpace(responseText))
            throw new InvalidOperationException("The chatbot returned an empty response.");

        history.Add($"NPC: {responseText}");
    }
}
