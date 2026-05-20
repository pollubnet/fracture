using Fracture.Server.Components.Popups;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Models;
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
    private readonly ItemDropStateService _itemDropState;

    private Position? _pendingPickupPosition;
    private Item? _pendingPickupItem;

    public MovementService(
        MapManagerService mapManagerService,
        IItemGenerator itemGenerator,
        IItemsRepository itemsRepository,
        UserService userService,
        ItemDropStateService itemDropState
    )
    {
        _mapManagerService = mapManagerService;
        _itemGenerator = itemGenerator;
        _itemsRepository = itemsRepository;
        _userService = userService;
        _itemDropState = itemDropState;
    }

    public Map? CurrentMap { get; private set; }

    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }

    public event EventHandler<Position>? OnMoved;
    public event EventHandler<(Map, Position)>? OnMapEntered;
    public event EventHandler<Position>? OnItemEncountered;
    public event EventHandler<Item>? OnItemPickupRequested;

    public async Task InitializeAsync()
    {
        CurrentMap =
            _mapManagerService.GetWorldMap()
            ?? throw new InvalidOperationException("Map cannot be loaded, critical error");

        var start = CurrentMap.GetRandomWalkableNode();
        CurrentX = start.X;
        CurrentY = start.Y;

        if (_userService.User != null)
        {
            await _itemDropState.EnsureLoadedAsync(_userService.User.Id);
        }

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

    public bool HasItemDrop(Map map, int x, int y)
    {
        if (_userService.User == null)
            return false;

        return _itemDropState.HasItemDrop(_userService.User.Id, map, x, y);
    }

    public async Task MoveAsync(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;

        if (CurrentMap != null && _userService.User != null)
        {
            var position = new Position(x, y);
            if (HasItemDrop(CurrentMap, x, y) && _pendingPickupPosition != position)
            {
                _pendingPickupPosition = position;
                _pendingPickupItem = await _itemGenerator.Generate();
                OnItemPickupRequested?.Invoke(this, _pendingPickupItem);
            }
        }

        OnMoved?.Invoke(this, new Position(CurrentX, CurrentY));
    }

    public async Task<bool> ConfirmPickupAsync()
    {
        if (CurrentMap == null || _userService.User == null || _pendingPickupPosition is null)
            return false;

        var position = _pendingPickupPosition.Value;
        var item = _pendingPickupItem;

        _pendingPickupPosition = null;
        _pendingPickupItem = null;

        if (item == null)
            return false;

        if (
            !await _itemDropState.TryCollectAsync(
                _userService.User.Id,
                CurrentMap,
                position.X,
                position.Y
            )
        )
            return false;

        item.CreatedById = _userService.User.Id;
        item.CreatedBy = _userService.User;

        await _itemsRepository.AddItemAsync(item);
        _userService.Inventory.Add(item);

        OnItemEncountered?.Invoke(this, position);
        return true;
    }

    public async Task CancelPickupAsync()
    {
        if (CurrentMap == null || _userService.User == null || _pendingPickupPosition is null)
            return;

        var position = _pendingPickupPosition.Value;

        _pendingPickupPosition = null;
        _pendingPickupItem = null;

        await _itemDropState.TryCollectAsync(
            _userService.User.Id,
            CurrentMap,
            position.X,
            position.Y
        );
    }
}
