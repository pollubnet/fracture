namespace Game.GameplaySession.Domain.Data.Enums
{
    /// <summary>
    /// The life state of the player, either dead or alive.
    /// TODO: This *should* map to a life state strategy class in the future, instead of being used directly.
    /// </summary>
    public enum PlayerLifeState
    {
        Alive,
        Dead
    }
}
