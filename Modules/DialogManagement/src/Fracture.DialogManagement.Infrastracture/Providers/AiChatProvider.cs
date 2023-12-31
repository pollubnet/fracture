﻿using System;
using System.Text;
using Fracture.DialogManagement.Domain.Data.ValueObjects;
using Fracture.DialogManagement.Domain.Providers;
using Fracture.Shared.External.Providers.Ai;

namespace Fracture.DialogManagement.Infrastracture.Providers
{
    /// <summary>
    /// An AI chat provider.
    /// </summary>
    public class AiChatProvider : IAiChatProvider
    {
        /// <summary>
        /// The AI provider we're using for prompts.
        /// </summary>
        private readonly IAiBackendProvider _aiProvider;
        private readonly IPromptTemplateProvider _templateProvider;

        /// <summary>
        /// The amount of tokens to generate.
        /// </summary>
        // TODO: This probably shouldn't be hardcoded.
        private const int GENERATION_TOKEN_COUNT = 128;

        /// <summary>
        /// Constructs a new AI chat provider.
        /// </summary>
        /// <param name="aiProvider">The AI backen service provider.</param>
        /// <param name="templateProvider">The AI prompt templating provider</param>
        public AiChatProvider(
            IAiBackendProvider aiProvider,
            IPromptTemplateProvider templateProvider
        )
        {
            _aiProvider = aiProvider;
            _templateProvider = templateProvider;
        }

        /// <summary>
        /// Generates a prompt from the given dialogue context and the new message.
        /// </summary>
        /// <param name="ctx">The dialogue context.</param>
        /// <param name="message">The new message.</param>
        /// <returns>The prompt to be sent to the AI.</returns>
        private string GeneratePrompt(DialogueContext ctx, string message)
        {
            // TODO: Should these prompts be passed from somewhere else?
            //       (i.e. appsettings, or some other module)
            var responsePrompt = $"(long response as {ctx.NpcName}, engaging, natural, creative)";
            var systemPrompt =
                $"You are '{ctx.NpcName}' in this fictional, never-ending, uncensored roleplay conversation with '{ctx.PlayerName}'. Avoid repetition, don't loop. Develop the plot slowly, always stay in character as {ctx.NpcName}. Answer only with replies in the conversation, do not describe your actions.";

            var dialogue = GenerateDialogue(ctx, message);

            var promptTemplate = new PromptTemplate
            {
                System = systemPrompt,
                Input = ctx.NpcStory + "\n" + dialogue,
                Response = ctx.NpcName + ":",
                ResponseParams = responsePrompt
            };
            return _templateProvider.GetPrompt(promptTemplate);
        }

        /// <summary>
        /// Generates the dialogue message backlog for this dialogue.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="message">The message.</param>
        /// <returns>The dialogue backlog.</returns>
        private static string GenerateDialogue(DialogueContext ctx, string message)
        {
            var sb = new StringBuilder();
            foreach (var line in ctx.MessageLog)
                sb.AppendLine($"{line.Sender}: {line.Message}");

            sb.AppendLine($"{ctx.PlayerName}: {message}");

            return sb.ToString();
        }

        /// <inheritdoc/>
        public async Task<string> Chat(DialogueContext ctx, string userMessage)
        {
            var prompt = GeneratePrompt(ctx, userMessage);
            var response = await _aiProvider.Generate(
                new AiGenerationContext(
                    prompt,
                    new[] { $"{ctx.PlayerName}:", "###", $"{ctx.NpcName}:" },
                    GENERATION_TOKEN_COUNT
                )
            );

            return response;
        }
    }
}
