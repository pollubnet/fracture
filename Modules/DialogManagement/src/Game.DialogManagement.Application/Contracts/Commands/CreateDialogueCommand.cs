using System;
using Game.DialogManagement.Domain.Data.Entities;
using MediatR;

namespace Game.DialogManagement.Application.Contracts.Commands
{
    /// <summary>
    /// The command responsible for creating a new Dialogue record.
    /// </summary>
    /// <param name="NpcId">The id of the NPC.</param>
    /// <param name="PlayerId">The id of the player.</param>
    /// <param name="NpcName">The name of the NPC.</param>
    /// <param name="NpcStory">The story of the NPC.</param>
    /// <param name="PlayerName">The name of the player.</param>
    // TODO: We should probably not pass these as-is, and wrap them in some record.
    //       (Not doing this yet, since we don't have the player nor the NPC modules)
    public record CreateDialogueCommand(
        Guid NpcId,
        Guid PlayerId,
        string NpcName,
        string NpcStory,
        string PlayerName
    ) : IRequest<Dialogue> { }
}
