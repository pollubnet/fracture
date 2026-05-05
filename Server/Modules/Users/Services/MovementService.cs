using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Services;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;

namespace Fracture.Server.Modules.Users.Services;

public class MovementService
{
    private readonly MapManagerService _mapManagerService;
    private readonly IItemGenerator _itemGenerator;
    private readonly IItemsRepository _itemsRepository;
    private readonly UserService _userService;

    public MovementService(
        MapManagerService mapManagerService,
        IItemGenerator itemGenerator,
        IItemsRepository itemsRepository,
        UserService userService
    )
    {
        _mapManagerService = mapManagerService;
        _itemGenerator = itemGenerator;
        _itemsRepository = itemsRepository;
        _userService = userService;
    }

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

        var start = CurrentMap.GetRandomWalkableNode();
        CurrentX = start.X;
        CurrentY = start.Y;

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

        var node = CurrentMap?.Grid[x, y];
        if (node?.ItemDrop != null && _userService.User != null)
        {
            var item = await _itemGenerator.Generate();
            item.CreatedById = _userService.User.Id;
            item.CreatedBy = _userService.User;

            await _itemsRepository.AddItemAsync(item);
            _userService.Inventory.Add(item);

            node.ItemDrop = null;
        }

        OnMoved?.Invoke(this, new Position(CurrentX, CurrentY));
    }
}
