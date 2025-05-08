using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.MapGenerator.UI;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImageChanger
{
    private readonly ILogger<BackgroundImageChanger> _logger;

    public BackgroundImageChanger(
        ILogger<BackgroundImageChanger> logger,
        MapManagerService mapManagerService
    )
    {
        this._logger = logger;
        MapManagerService = mapManagerService;
    }

    public required BackgroundImage? BackgroundImage { get; set; }
    public required MapManagerService MapManagerService { get; set; }

    public Task ChangeBackgroundImageAsync()
    {
        if (BackgroundImage != null)
        {
            BackgroundImage.ImagePath = MapManagerService
                .CurrentMap
                .Grid[MapView.CharacterXX, MapView.CharacterYY]
                .Biome
                .BackgroundImageUrl;
        }
        else
        {
            _logger.LogError("BackgroundImage is missing");
        }

        return Task.CompletedTask;
    }

    public event EventHandler<EventArgs>? BackgroundImageChanged;

    public void NotifyListChanged(object? sender, EventArgs e)
    {
        BackgroundImageChanged?.Invoke(sender, e);
    }
}
