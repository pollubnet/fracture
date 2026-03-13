using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;

namespace Fracture.Server.Modules.Users.Services;

public class MovementService(UserService _userService, MapManagerService _mapManagerService)
{
    public Map? CurrentMap { get; private set; }

    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }

    public event EventHandler<Position>? OnMoved;
    public event EventHandler<(Map, Position)>? OnMapEntered;

    public void Initialize()
    {
        CurrentMap =
            _mapManagerService.GetWorldMap()
            ?? throw new InvalidOperationException("Map cannot be loaded, critical error");

        CurrentX = 16;
        CurrentY = 16;

        OnMapEntered?.Invoke(this, new(CurrentMap, new Position(CurrentX, CurrentY)));
    }

    public bool CanMove(int x, int y)
    {
        return CurrentMap != null
            && x >= 0
            && y >= 0
            && x < CurrentMap!.Width
            && y < CurrentMap.Height
            && CurrentMap.Grid[x, y].Walkable;
    }

    public async Task MoveAsync(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;

        OnMoved?.Invoke(this, new Position(CurrentX, CurrentY));
    }
}
