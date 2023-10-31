using System;
using Game.Shared.Core.Data;

namespace Game.DialogManagement.Domain.Data.ValueObjects
{
    /// <summary>
    /// The dialogue context, containing all of the information neccessary to
    /// actually converse.
    /// </summary>
    /// <param name="NpcName">The name of the NPC we're talking with.</param>
    /// <param name="NpcStory">The backstory of said NPC.</param>
    /// <param name="PlayerName">The name of the player talking.</param>
    /// <param name="Backlog">The backlog of messages.</param>
    public record DialogueContext(
        string NpcName,
        string NpcStory,
        string PlayerName,
        List<string> Backlog
    ) : ValueObject { }
}
