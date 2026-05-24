using Fracture.Server.Components.Popups;
using Fracture.Server.Components.UI;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.UI.Models;
using Fracture.Server.Modules.Pathfinding.Models;
using Fracture.Server.Modules.Users.Models;
using Fracture.Server.Modules.Users.Services;

namespace Fracture.Server.Components.Pages;

public partial class GamePage : IAsyncDisposable
{
    private Dictionary<string, object> _mapPopupParameters = null!;

    private PopupContainer _popup = null!;

    public string BackgroundImage { get; set; } = string.Empty;

    private readonly MapDisplayOptions _mapDisplayOptions = new();

    private List<IPathfindingNode>? Path { get; set; }

    protected override async Task OnInitializedAsync()
    {
        bool userLoaded = await LoadUserAsync();
        if (!userLoaded)
        {
            NavigationManager.NavigateTo("/");
        }

        if (MovementService.CurrentMap is null)
        {
            await MovementService.InitializeAsync();
            BackgroundImage = GetBackgroundImagePath();

            // Załaduj zapisaną pozycję gracza (jeśli istnieje)
            if (UserService.User != null)
            {
                await LoadPlayerPositionAsync();
            }
        }

        MovementService.OnMapEntered += async (sender, args) =>
        {
            BackgroundImage = GetBackgroundImagePath();
            StateHasChanged();
        };

        MovementService.OnMoved += async (sender, args) =>
        {
            BackgroundImage = GetBackgroundImagePath();
            StateHasChanged();
        };

        MovementService.OnItemPickupRequested += async (sender, item) =>
        {
            var parameters = new Dictionary<string, object>
            {
                { "Item", item },
                {
                    "OnConfirm",
                    (Func<Task>)(
                        async () =>
                        {
                            await MovementService.ConfirmPickupAsync();
                            _popup.Hide();
                        }
                    )
                },
                {
                    "OnCancel",
                    (Func<Task>)(
                        async () =>
                        {
                            await MovementService.CancelPickupAsync();
                            _popup.Hide();
                        }
                    )
                },
            };

            _popup.ShowComponent<ItemPickupRequest>(parameters);
        };

        _mapDisplayOptions.ShowColorMap = true;
        _mapPopupParameters = new Dictionary<string, object>
        {
            { "MapDisplayData", _mapDisplayOptions },
        };

        await base.OnInitializedAsync();
    }

    private async Task<bool> LoadUserAsync()
    {
        var username = await ProtectedSessionStore.GetAsync<string>("username");
        if (!username.Success)
        {
            return false;
        }

        if (string.IsNullOrEmpty(username.Value))
        {
            return false;
        }

        var user = await UsersRepository.GetUserAsync(username.Value!);
        if (user is null)
        {
            user = new User { Username = username.Value! };
            await UsersRepository.AddUserAsync(user);
        }

        await UserService.LoadUserAsync(user);
        return true;
    }

    private async Task LoadPlayerPositionAsync()
    {
        if (UserService.User == null)
            return;

        var positionString = await UserService.GetPlayerPositionAsync(UserService.User.Id);

        if (string.IsNullOrEmpty(positionString))
            return;

        var parts = positionString.Split(',');
        if (
            parts.Length == 2
            && int.TryParse(parts[0], out int x)
            && int.TryParse(parts[1], out int y)
        )
        {
            if (MovementService.CanMove(x, y))
            {
                MovementService.CurrentX = x;
                MovementService.CurrentY = y;
                Logger.LogInformation($"Loaded player position: ({x}, {y})");
            }
        }
    }

    private async Task SavePlayerPositionAsync()
    {
        if (UserService.User != null && MovementService.CurrentMap != null)
        {
            var position = $"{MovementService.CurrentX},{MovementService.CurrentY}";
            await UserService.UpdatePlayerPositionAsync(UserService.User.Id, position);
            Logger.LogInformation($"Saved player position: {position}");
        }
    }

    private async Task LogoutAsync()
    {
        // Zapisz pozycję przed wylogowaniem
        await SavePlayerPositionAsync();

        await ProtectedSessionStore.DeleteAsync("username");
        NavigationManager.NavigateTo("/home");
    }

    public async ValueTask DisposeAsync()
    {
        // Zapisz pozycję gdy gracz zamknie przeglądarkę
        await SavePlayerPositionAsync();
        GC.SuppressFinalize(this);
    }

    private string GetBackgroundImagePath()
    {
        if (MovementService.CurrentMap is null)
        {
            Logger.LogError("Current map is null");
            return string.Empty;
        }

        if (
            MovementService.CurrentX < 0
            || MovementService.CurrentY < 0
            || MovementService.CurrentX >= MovementService.CurrentMap.Width
            || MovementService.CurrentY >= MovementService.CurrentMap.Height
        )
        {
            Logger.LogError("Character is out of map");
            return string.Empty;
        }
        var cell = MovementService.CurrentMap.Grid[
            MovementService.CurrentX,
            MovementService.CurrentY
        ];
        var biome = cell.Biome;

        if (biome is null)
        {
            Logger.LogError("Biome is null");
            return string.Empty;
        }

        string? imagePath = null;
        if (cell.LocationType != LocationType.None)
        {
            var currentLocationName = cell.LocationType.ToString();
            var location = biome.Locations.FirstOrDefault(l =>
                string.Equals(l.Name, currentLocationName, StringComparison.OrdinalIgnoreCase)
            );

            if (location is null)
            {
                Logger.LogWarning(
                    "No matching location config for LocationType {LocationType} at ({X},{Y}) in biome {BiomeName}. Available: {Locations}",
                    cell.LocationType,
                    MovementService.CurrentX,
                    MovementService.CurrentY,
                    biome.Name,
                    string.Join(
                        ", ",
                        biome
                            .Locations.Where(l => !string.IsNullOrWhiteSpace(l.Name))
                            .Select(l => l.Name)
                    )
                );
            }
            else
            {
                imagePath = location.BackgroundImage;
                Logger.LogDebug(
                    "Using location background image '{ImagePath}' for LocationType {LocationType} at ({X},{Y})",
                    imagePath,
                    cell.LocationType,
                    MovementService.CurrentX,
                    MovementService.CurrentY
                );
            }
        }
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            imagePath = biome.BackgroundImage;
            Logger.LogDebug(
                "Using biome background image '{ImagePath}' for biome {BiomeName} at ({X},{Y})",
                imagePath,
                biome.Name,
                MovementService.CurrentX,
                MovementService.CurrentY
            );
        }

        if (string.IsNullOrWhiteSpace(imagePath))
        {
            Logger.LogError("Background image path is null or empty");
            return string.Empty;
        }

        return imagePath;
    }
}
