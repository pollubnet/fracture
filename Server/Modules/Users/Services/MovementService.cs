using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;

namespace Fracture.Server.Modules.Users.Services;

public class MovementService(UserService _userService, MapManagerService _mapManagerService)
{
    public Map? CurrentMap { get; private set; }

    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }

    public void Initialize()
    {
        CurrentMap =
            _mapManagerService.GetWorldMap()
            ?? throw new InvalidOperationException("Map cannot be loaded, critical error");
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
        await Task.Delay(100);
        CurrentX = x;
        CurrentY = y;
    }
}
