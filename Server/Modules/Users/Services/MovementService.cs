using Fracture.Server.Components.Popups;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Items.Services;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.Npc.Models;
using Fracture.Server.Modules.Shared;
using Microsoft.Extensions.Logging;

namespace Fracture.Server.Modules.Users.Services;

public class MovementService
{
    private readonly MapManagerService _mapManagerService;
    private readonly IItemGenerator _itemGenerator;
    private readonly IItemsRepository _itemsRepository;
    private readonly UserService _userService;
    private readonly ItemDropStateService _itemDropState;
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<MovementService> _logger;
    private readonly GameLogService _gameLogService;
    private Position? _pendingPickupPosition;
    private Item? _pendingPickupItem;

    // Autosave fields
    private CancellationTokenSource? _autosaveCts;
    private int _lastSavedX;
    private int _lastSavedY;
    private const int AUTOSAVE_INTERVAL_MS = 120000; // 2 minuty

    public MovementService(
        MapManagerService mapManagerService,
        IItemGenerator itemGenerator,
        IItemsRepository itemsRepository,
        UserService userService,
        ItemDropStateService itemDropState,
        IUsersRepository usersRepository,
        ILogger<MovementService> logger,
        GameLogService gameLogService
    )
    {
        _mapManagerService = mapManagerService;
        _itemGenerator = itemGenerator;
        _itemsRepository = itemsRepository;
        _userService = userService;
        _itemDropState = itemDropState;
        _usersRepository = usersRepository;
        _logger = logger;
        _gameLogService = gameLogService;
    }

    /// <summary>
    /// Reference to a current Map (array of Nodes)
    /// </summary>
    public Map? CurrentMap { get; private set; }

    /// <summary>
    /// Current player position
    /// </summary>
    public int CurrentX { get; private set; }

    /// <summary>
    /// Current player position
    /// </summary>
    public int CurrentY { get; private set; }

    /// <summary>
    /// Sets if the user can move
    /// </summary>
    public bool IsMovementLocked { get; private set; }

    /// <summary>
    /// Event fired when the user moved
    /// </summary>
    public event EventHandler<Position>? OnMoved;

    /// <summary>
    /// Event fired when the user entered a new map
    /// </summary>
    public event EventHandler<(Map, Position)>? OnMapEntered;

    /// <summary>
    /// Event fired when new item has been encountered
    /// </summary>
    public event EventHandler<Position>? OnItemEncountered;

    /// <summary>
    /// Event fired when item pickup has been requested
    /// </summary>
    public event EventHandler<Item>? OnItemPickupRequested;

    /// <summary>
    /// Event fired when an NPC is encountered
    /// </summary>
    public event EventHandler<NpcEncounter>? OnNpcEncountered;

    public async Task InitializeAsync()
    {
        CurrentMap =
            _mapManagerService.GetWorldMap()
            ?? throw new InvalidOperationException("Map cannot be loaded, critical error");

        // load player position from database or set to random walkable node if not available
        await LoadPlayerPositionAsync();

        OnMapEntered?.Invoke(this, (CurrentMap, new Position(CurrentX, CurrentY)));

        // start autosave loop
        StartAutosave();
    }

    /// <summary>
    /// Loads the player's position from the database. If no position is saved, it sets a random walkable position on the current map.
    /// </summary>
    public async Task LoadPlayerPositionAsync()
    {
        if (_userService.User?.PlayerPosition is null)
        {
            // if no position is saved, set a random walkable position on the current map
            CurrentX = CurrentMap!.GetRandomWalkableNode().X;
            CurrentY = CurrentMap!.GetRandomWalkableNode().Y;
            return;
        }

        var parts = _userService.User.PlayerPosition.Split(',');
        if (
            parts.Length == 2
            && int.TryParse(parts[0], out var x)
            && int.TryParse(parts[1], out var y)
        )
        {
            if (CanMove(x, y))
            {
                CurrentX = x;
                CurrentY = y;
                _lastSavedX = x;
                _lastSavedY = y;
                _logger.LogInformation($"Loaded player position: ({x}, {y})");
            }
            else
            {
                // position is invalid, set to a random walkable position on the current map
                var randomPos = CurrentMap!.GetRandomWalkableNode();
                CurrentX = randomPos.X;
                CurrentY = randomPos.Y;
                _lastSavedX = randomPos.X;
                _lastSavedY = randomPos.Y;
            }
        }
    }

    /// <summary>
    /// Saves the player's current position to the database
    /// </summary>
    public async Task SavePlayerPositionAsync()
    {
        if (_userService.User is null)
            return;

        _userService.User.PlayerPosition = $"{CurrentX},{CurrentY}";
        await _usersRepository.SaveAsync();
    }

    /// <summary>
    /// Starts the autosave loop for the player's position
    /// </summary>
    public void StartAutosave()
    {
        _autosaveCts = new CancellationTokenSource();
        _ = AutosavePositionAsync(_autosaveCts.Token);
    }

    /// <summary>
    /// Stops the autosave loop
    /// </summary>
    public void StopAutosave()
    {
        _autosaveCts?.Cancel();
        _autosaveCts?.Dispose();
        _autosaveCts = null;
    }

    /// <summary>
    /// The autosave loop
    /// </summary>
    private async Task AutosavePositionAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(AUTOSAVE_INTERVAL_MS, cancellationToken);
            if (CurrentX != _lastSavedX || CurrentY != _lastSavedY)
            {
                await SavePlayerPositionAsync();
                _lastSavedX = CurrentX;
                _lastSavedY = CurrentY;
                _logger.LogInformation(
                    $"Autosave triggered - position changed: ({_lastSavedX}, {_lastSavedY})"
                );
            }
            else
            {
                _logger.LogInformation("Autosave skipped - position unchanged");
            }
        }
    }

    /// <summary>
    /// Reloads the player's position (for new users)
    /// </summary>
    public async Task ReloadPlayerPositionAsync()
    {
        _lastSavedX = CurrentX;
        _lastSavedY = CurrentY;
        await LoadPlayerPositionAsync();
    }

    /// <summary>
    /// Checks if the user can move to that new position
    /// </summary>
    /// <param name="x">New X on a current map</param>
    /// <param name="y">New Y on a current map</param>
    /// <returns>Returns false if the user cannot move or they are trying to move out of the map or to an unwalkable node</returns>
    public bool CanMove(int x, int y)
    {
        if (IsMovementLocked)
        {
            return false;
        }

        return CurrentMap != null
            && x >= 0
            && y >= 0
            && x < CurrentMap!.Width
            && y < CurrentMap.Height
            && CurrentMap.Grid[x, y].Walkable;
    }

    /// <summary>
    /// Checks if there is an Item Drop on this position
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool HasItemDrop(Map map, int x, int y)
    {
        if (_userService.User == null)
            return false;

        return _itemDropState.HasItemDrop(_userService.User.Id, map, x, y);
    }

    /// <summary>
    /// Moves the player into a new position
    /// </summary>
    /// <param name="x">New X</param>
    /// <param name="y">New Y</param>
    /// <returns></returns>
    public async Task MoveAsync(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;

        if (CurrentMap != null && _userService.User != null)
        {
            var position = new Position(x, y);

            // if there is an item drop at the new position and it's not the same as the pending pickup position, generate a new item and request pickup
            if (HasItemDrop(CurrentMap, x, y) && _pendingPickupPosition != position)
            {
                _pendingPickupPosition = position;
                _pendingPickupItem = await _itemGenerator.Generate();

                OnItemPickupRequested?.Invoke(this, _pendingPickupItem);
            }

            // NPC encounter
            if (CurrentMap.Grid[x, y].MapObject is NpcMapObject npc)
            {
                OnNpcEncountered?.Invoke(this, new NpcEncounter(npc, npc.Entity, position));
            }
        }

        OnMoved?.Invoke(this, new Position(CurrentX, CurrentY));
    }

    /// <summary>
    /// Handles item pickup request
    /// </summary>
    /// <returns></returns>
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

        await _gameLogService.LogAsync($"Item Name: {item.Name}", GameLogType.NewItem);
        OnItemEncountered?.Invoke(this, position);
        IsMovementLocked = false;
        return true;
    }

    /// <summary>
    /// Cancels the item pickup
    /// </summary>
    /// <returns></returns>
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
        IsMovementLocked = false;
    }

    /// <summary>
    /// Requests the item pickup popup
    /// </summary>
    /// <returns></returns>
    public async Task RequestItemPickup()
    {
        IsMovementLocked = true;
    }

    /// <summary>
    /// Requests the npc encounter popup
    /// </summary>
    /// <returns></returns>
    public async Task RequestNpcEncounter()
    {
        IsMovementLocked = true;
    }

    /// <summary>
    /// Starts the NPC encounter
    /// </summary>
    /// <returns></returns>
    public async Task ConfirmNpcEncounterAsync()
    {
        IsMovementLocked = true;
    }

    /// <summary>
    /// Cancels the NPC encounter
    /// </summary>
    /// <returns></returns>
    public async Task CancelNpcEncounterAsync()
    {
        IsMovementLocked = false;
    }

    /// <summary>
    /// Finishes the NPC encounter
    /// </summary>
    /// <returns></returns>
    public async Task FinishNpcEncounterAsync()
    {
        IsMovementLocked = false;
    }
}
