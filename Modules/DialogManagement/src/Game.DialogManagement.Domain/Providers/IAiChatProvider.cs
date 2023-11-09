using System;
using Game.DialogManagement.Domain.Data.ValueObjects;

namespace Game.DialogManagement.Domain.Providers
{
    /// <summary>
    /// The interface implemented by anything that wants to provide a means to
    /// interact with an AI backend.
    /// </summary>
    public interface IAiChatProvider
    {
        /// <summary>
        /// Sends a chat message and retrieves the next predicted one.
        /// </summary>
        /// <param name="ctx">The context of the dialogue.</param>
        /// <param name="userMessage">The message the user has sent.</param>
        /// <returns>The next predicted dialogue message.</returns>
        Task<string> Chat(DialogueContext ctx, string userMessage);
    }
}
