using System;
using Game.Shared.Core.Data;

namespace Game.DialogManagement.Domain.Data.ValueObjects
{
    /// <summary>
    /// The dialogue context, containing all of the information neccessary to
    /// actually converse.
    /// </summary>
    public class DialogueContext : Entity
    {
        /// <summary>
        /// The name of the NPC we're conversing with.
        /// </summary>
        public required string NpcName { get; init; } = null!;

        /// <summary>
        /// The NPC's backstory.
        /// </summary>
        public required string NpcStory { get; init; } = null!;

        /// <summary>
        /// The name of the player that's conversing with the player.
        /// </summary>
        public required string PlayerName { get; init; } = null!;

        /// <summary>
        /// The message log for this dialogue context.
        /// </summary>
        public List<DialogueMessage> MessageLog { get; init; } = null!;

        /// <summary>
        /// Constructs a default dialogue context.
        /// </summary>
        public DialogueContext()
        {
            MessageLog = new();
        }
    }
}
