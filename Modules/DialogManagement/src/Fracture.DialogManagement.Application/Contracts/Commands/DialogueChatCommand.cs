using System;
using MediatR;
using Fracture.DialogManagement.Domain.Data.Entities;

namespace Fracture.DialogManagement.Application.Contracts.Commands
{
    /// <summary>
    /// A command for generating the next message by the NPC from a dialogue.
    /// </summary>
    /// <param name="Dialogue">The dialogue we're currently in.</param>
    /// <param name="UserMessage">The message the user has sent.</param>
    public record DialogueChatCommand(Dialogue Dialogue, string UserMessage) : IRequest<string> { }
}
