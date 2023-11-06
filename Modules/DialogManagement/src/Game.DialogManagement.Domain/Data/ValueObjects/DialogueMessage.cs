using System;
using Game.Shared.Core.Data;

namespace Game.DialogManagement.Domain.Data.ValueObjects
{
    /// <summary>
    /// A single dialogue message.
    /// </summary>
    /// <param name="Message">The message.</param>
    public record DialogueMessage(string Message) : ValueObject { }
}
