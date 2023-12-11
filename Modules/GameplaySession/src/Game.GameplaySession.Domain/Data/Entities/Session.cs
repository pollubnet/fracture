using Game.GameplaySession.Domain.Data.Enums;
using Game.GameplaySession.Domain.Data.ValueObjects;
using Game.Shared.Core.Data;

namespace Game.GameplaySession.Domain.Data.Entities
{
    /// <summary>
    /// The gameplay session entity, managing most of the state.
    /// </summary>
    public class Session : Entity
    {
        /// <summary>
        /// The ID of the player owning this session.
        /// </summary>
        public required Guid PlayerId { get; init; }

        /// <summary>
        /// The position of the player.
        /// </summary>
        public Position PlayerPosition { get; private set; } = new(0, 0);

        /// <summary>
        /// The life state of the player.
        /// </summary>
        public PlayerLifeState LifeState { get; private set; } = PlayerLifeState.Alive;

        /// <summary>
        /// Moves the player to a given position.
        /// </summary>
        /// <param name="pos">The position.</param>
        public void MovePlayerTo(Position pos)
        {
            PlayerPosition = pos;
        }

        /// <summary>
        /// Marks the player as dead.
        /// </summary>
        public void MarkDead()
        {
            LifeState = PlayerLifeState.Dead;
        }
    }
}
