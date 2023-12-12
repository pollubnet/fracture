using System;
using Fracture.Shared.Core.Data;

namespace Fracture.DialogManagement.Domain.Data.ValueObjects
{
    /// <summary>
    /// A single dialogue message.
    /// </summary>
    /// <param name="Sender">The message sender.</param>
    /// <param name="Message">The message.</param>
    public record DialogueMessage(string Sender, string Message) : ValueObject { }
}
