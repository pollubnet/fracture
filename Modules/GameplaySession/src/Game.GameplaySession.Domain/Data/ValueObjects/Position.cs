using Game.Shared.Core.Data;

namespace Game.GameplaySession.Domain.Data.ValueObjects
{
    /// <summary>
    /// The position of the player, in XY coordinates.
    /// </summary>
    /// <param name="X">The X coordinate of the position.</param>
    /// <param name="Y">The Y coordinate of the position.</param>
    public record Position(int X, int Y) : ValueObject;
}
