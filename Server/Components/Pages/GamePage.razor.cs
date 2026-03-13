using Fracture.Server.Components.UI;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.UI.Models;
using Fracture.Server.Modules.Pathfinding.Models;
using Fracture.Server.Modules.Users.Models;
using Fracture.Server.Modules.Users.Services;

namespace Fracture.Server.Components.Pages;

public partial class GamePage
{
    private Dictionary<string, object> _mapPopupParameters = null!;

    private PopupContainer _popup = null!;

    public string BackgroundImage { get; set; } = string.Empty;

    private readonly MapDisplayOptions _mapDisplayOptions = new();

    private List<IPathfindingNode>? Path { get; set; }

    protected override async Task OnInitializedAsync()
    {
        bool userLoaded = await LoadUser();
        if (!userLoaded)
        {
            NavigationManager.NavigateTo("/");
        }

        if (MovementService.CurrentMap is null)
        {
            MovementService.Initialize();
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

        _mapDisplayOptions.ShowColorMap = true;
        _mapPopupParameters = new Dictionary<string, object>
        {
            { "MapDisplayData", _mapDisplayOptions },
        };

        await base.OnInitializedAsync();
    }

    private async Task<bool> LoadUser()
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

    private void Logout()
    {
        NavigationManager.NavigateTo("/home");
        ProtectedSessionStore.DeleteAsync("username");
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
