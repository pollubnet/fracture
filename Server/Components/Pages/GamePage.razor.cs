using Fracture.Server.Components.UI;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.UI.Models;
using Fracture.Server.Modules.Pathfinding.Models;
using Fracture.Server.Modules.Shared.ImageChangers;
using Fracture.Server.Modules.Users.Models;

namespace Fracture.Server.Components.Pages;

public partial class GamePage
{
    private Dictionary<string, object> _mapPopupParameters = null!;
    public static Map Map { get; set; }

    private PopupContainer _popup = null!;

    public static BackgroundImage BackgroundImage { get; set; } = new(string.Empty);

    private readonly MapDisplayData _mapDisplayData = new();

    private List<IPathfindingNode>? Path { get; set; }

    protected override async Task OnInitializedAsync()
    {
        bool userLoaded = await LoadUser();
        if (!userLoaded)
        {
            NavigationManager.NavigateTo("/");
        }

        Map = MapManagerService.GetWorldMap() ?? throw new InvalidOperationException();
        _mapDisplayData.ShowColorMap = true;
        _mapPopupParameters = new Dictionary<string, object>
        {
            { "Map", Map },
            { "MapDisplayData", _mapDisplayData },
        };

        BackgroundImageChanger.BackgroundImage = BackgroundImage;
        await GetBacgroundAsync();

        BackgroundImageChanger.BackgroundImageChanged += OnBgChanged!;

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

    //this piece of codes refreshes GamePage after changing background image. It is necessary to show new background.
    private async Task GetBacgroundAsync()
    {
        await BackgroundImageChanger.ChangeBackgroundImageAsync();
    }

    private void OnBgChanged(object sender, EventArgs e)
    {
        this.InvokeAsync(this.StateHasChanged);
    }

    public void Dispose()
    {
        BackgroundImageChanger.BackgroundImageChanged -= OnBgChanged!;
    }
}
