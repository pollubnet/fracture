using Fracture.Server.Components.Popups;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Items.Services;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;
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
        ILogger<MovementService> logger
    )
    {
        _mapManagerService = mapManagerService;
        _itemGenerator = itemGenerator;
        _itemsRepository = itemsRepository;
        _userService = userService;
        _itemDropState = itemDropState;
        _usersRepository = usersRepository;
        _logger = logger;
    }

    public Map? CurrentMap { get; private set; }

    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }
    public bool IsMovementLocked { get; private set; }

    public event EventHandler<Position>? OnMoved;
    public event EventHandler<(Map, Position)>? OnMapEntered;
    public event EventHandler<Position>? OnItemEncountered;
    public event EventHandler<Item>? OnItemPickupRequested;

    public async Task InitializeAsync()
    {
        CurrentMap =
            _mapManagerService.GetWorldMap()
            ?? throw new InvalidOperationException("Map cannot be loaded, critical error");

        // Wczytaj zapisaną pozycję gracza
        await LoadPlayerPositionAsync();

        OnMapEntered?.Invoke(this, (CurrentMap, new Position(CurrentX, CurrentY)));

        // Uruchom autosave automatycznie
        StartAutosave();
    }

    /// <summary>
    /// Wczytuje pozycję gracza z bazy danych
    /// </summary>
    public async Task LoadPlayerPositionAsync()
    {
        if (_userService.User?.PlayerPosition is null)
        {
            // Jeśli nie ma zapisanej pozycji, ustaw pozycję startową
            CurrentX = CurrentMap?.GetRandomWalkableNode().X ?? 0;
            CurrentY = CurrentMap?.GetRandomWalkableNode().Y ?? 0;
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
                // Pozycja nie jest walkable - losuj nową
                var randomPos = CurrentMap.GetRandomWalkableNode();
                CurrentX = randomPos.X;
                CurrentY = randomPos.Y;
                _lastSavedX = randomPos.X;
                _lastSavedY = randomPos.Y;
            }
        }
    }

    /// <summary>
    /// Zapisuje aktualną pozycję gracza do bazy danych
    /// </summary>
    public async Task SavePlayerPositionAsync()
    {
        if (_userService.User is null)
            return;

        _userService.User.PlayerPosition = $"{CurrentX},{CurrentY}";
        await _usersRepository.SaveAsync();
    }

    /// <summary>
    /// Uruchamia autosave pozycji gracza co 2 minuty
    /// </summary>
    public void StartAutosave()
    {
        _autosaveCts = new CancellationTokenSource();
        _ = AutosavePositionAsync(_autosaveCts.Token);
    }

    /// <summary>
    /// Zatrzymuje autosave
    /// </summary>
    public void StopAutosave()
    {
        _autosaveCts?.Cancel();
        _autosaveCts?.Dispose();
        _autosaveCts = null;
    }

    /// <summary>
    /// Pętla autosave'a
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
    /// Przeładowuje pozycję gracza (dla nowego użytkownika)
    /// </summary>
    public async Task ReloadPlayerPositionAsync()
    {
        _lastSavedX = CurrentX;
        _lastSavedY = CurrentY;
        await LoadPlayerPositionAsync();
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
        if (IsMovementLocked)
        {
            return;
        }
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
        IsMovementLocked = false;
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
        IsMovementLocked = false;
    }

    public async Task RequestItemPickup()
    {
        IsMovementLocked = true;
    }
}
