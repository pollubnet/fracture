using System;
using Game.DialogManagement.Domain.Data.Entities;

namespace Game.DialogManagement.Domain.Repositories
{
    /// <summary>
    /// The interface implemented by anything that wants to store dialogues.
    /// </summary>
    public interface IDialogueRepository
    {
        /// <summary>
        /// Gets a dialogue from the repository given the npc id and the player id.
        /// </summary>
        /// <param name="npcId">The id of the NPC we're conversing with.</param>
        /// <param name="playerId">The id of the player conversing.</param>
        /// <returns>Either the dialogue, or nothing if one doesn't exist.</returns>
        Task<Dialogue?> Get(Guid npcId, Guid playerId);

        /// <summary>
        /// Creates a dialogue in the repository.
        /// </summary>
        /// <param name="dialogue">The dialogue to create.</param>
        /// <returns>Whether the creation of the dialogue has succeeded.</returns>
        Task<bool> Create(Dialogue dialogue);

        /// <summary>
        /// Updates the dialogue in the repository.
        /// </summary>
        /// <param name="dialogue">The dialogue to be updated.</param>
        /// <returns>Whether the update has succeeded.</returns>
        Task<bool> Update(Dialogue dialogue);
    }
}
