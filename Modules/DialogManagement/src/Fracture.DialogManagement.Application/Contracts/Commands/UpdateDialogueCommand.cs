using System;
using Fracture.DialogManagement.Domain.Data.Entities;
using MediatR;

namespace Fracture.DialogManagement.Application.Contracts.Commands
{
    /// <summary>
    /// The command responsible for updating a dialogue.
    /// </summary>
    /// <param name="Dialogue">The dialogue to be updated.</param>
    public record UpdateDialogueCommand(Dialogue Dialogue) : IRequest { }
}
