using System;

namespace Fracture.DialogManagement.Api.Contracts.Requests
{
    /// <summary>
    /// The data sent in by the client when they want to talk with an NPC.
    /// </summary>
    /// <param name="PlayerId">The player's ID.</param>
    /// <param name="Message">The message they're sending.</param>
    public record TalkRequest(Guid PlayerId, string Message) { }
}
