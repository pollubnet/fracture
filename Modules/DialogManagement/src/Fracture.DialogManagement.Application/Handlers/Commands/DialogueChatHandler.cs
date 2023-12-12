using System;
using MediatR;
using Fracture.DialogManagement.Application.Contracts.Commands;
using Fracture.DialogManagement.Domain.Providers;

namespace Fracture.DialogManagement.Application.Handlers.Commands
{
    /// <summary>
    /// The handler for the DialogueChat command.
    /// </summary>
    public class DialogueChatHandler : IRequestHandler<DialogueChatCommand, string>
    {
        /// <summary>
        /// The AI provider for the chat functionality.
        /// </summary>
        private readonly IAiChatProvider _aiChatProvider;

        /// <summary>
        /// Constructs a new dialogue chat handler for a given AI provider.
        /// </summary>
        /// <param name="aiChatProvider">The AI provider for the chat.</param>
        public DialogueChatHandler(IAiChatProvider aiChatProvider)
        {
            _aiChatProvider = aiChatProvider;
        }

        /// <inheritdoc/>
        public async Task<string> Handle(
            DialogueChatCommand request,
            CancellationToken cancellationToken
        )
        {
            var response = await _aiChatProvider.Chat(
                request.Dialogue.Context,
                request.UserMessage
            );
            return response;
        }
    }
}
