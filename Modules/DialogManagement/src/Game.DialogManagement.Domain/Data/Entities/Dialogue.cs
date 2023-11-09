using System;
using Game.DialogManagement.Domain.Data.ValueObjects;
using Game.Shared.Core.Data;

namespace Game.DialogManagement.Domain.Data.Entities
{
    /// <summary>
    /// The main dialogue entity.
    /// </summary>
    public class Dialogue : Entity
    {
        /// <summary>
        /// The ID of the player that initiated the dialogue.
        /// </summary>
        public Guid PlayerId { get; private set; }

        /// <summary>
        /// The ID of the NPC that the player is conversing with.
        /// </summary>
        public Guid NpcId { get; private set; }

        /// <summary>
        /// The context of the dialogue.
        /// </summary>
        public DialogueContext Context { get; private set; } = null!;

        /// <summary>
        /// Creates a new dialogue for a given player and npc.
        /// </summary>
        /// <param name="playerId">The player's id.</param>
        /// <param name="npcId">The npc's id.</param>
        /// <param name="context">The dialogue context.</param>
        public Dialogue(Guid playerId, Guid npcId, DialogueContext context)
        {
            PlayerId = playerId;
            NpcId = npcId;

            Context = context;
        }

        /// <summary>
        /// Adds a message to this dialogue's context backlog.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="message">The message.</param>
        public void AddMessageToBacklog(string sender, string message)
        {
            Context.MessageLog.Add(new(sender, message));
        }
    }
}
