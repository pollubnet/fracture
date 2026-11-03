using Fracture.Server.Modules.Items.Services;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.Npc.Models;
using Fracture.Server.Modules.Serial;
using Fracture.Server.Modules.Shared;
using Fracture.Server.Modules.Users.Services;
using Microsoft.Extensions.AI;

namespace Fracture.Server.Modules.Npc.Services;

public class NpcDialogueService(
    WorldKnowledgeService worldKnowledgeService,
    IItemGenerator itemGenerator,
    UserService userService,
    ILogger<NpcDialogueService> logger,
    GameLogService gameLogService,
    ISerialCommandService serialCommandService,
    IChatClient? chatClient = null
)
{
    public async Task SendMessageAsync(NpcEntity npc, List<string> history)
    {
        if (chatClient is null)
            throw new InvalidOperationException("The chatbot is not available.");

        var chatOptions = new ChatOptions
        {
            Tools =
            [
                AIFunctionFactory.Create(
                    async () =>
                    {
                        var item = await itemGenerator.Generate();
                        logger.LogInformation(
                            "Generated item {ItemName} for user {UserId}",
                            item.Name,
                            userService.User?.Id
                        );
                        userService.Inventory.Add(item);
                        await gameLogService.LogAsync($"You received a new item: {item.Name}");
                        return item.Name;
                    },
                    name: "generate_item",
                    description: "Creates a new item and gives it to the player, returns its name"
                ),
                AIFunctionFactory.Create(
                    (int r, int g, int b) =>
                    {
                        serialCommandService.Send($"{r} {g} {b}\n");
                    },
                    name: "color_led",
                    description: "Controls the color of an LED"
                ),
            ],
            Seed = RandomProvider.Seed,
        };

        var systemMessage = $"""
            You are {npc.Name}, {npc.Role} a character in a fantasy role-playing game.

            Your history: {npc.History}

            Facts about the world: {worldKnowledgeService.Knowledge}

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
        var response = await chatClient.GetResponseAsync(chatMessages, chatOptions);
        //var response = await chatClient.GetResponseAsync(chatMessages);
        var responseText = response.Text?.Trim();
        if (string.IsNullOrWhiteSpace(responseText))
            throw new InvalidOperationException("The chatbot returned an empty response.");

        history.Add($"NPC: {responseText}");
    }
}
