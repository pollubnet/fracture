using System.Collections.ObjectModel;
using Fracture.Server.Components.UI;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.UI.Models;
using Fracture.Server.Modules.Pathfinding.Models;
using Fracture.Server.Modules.Shared.ImageChangers;
using Fracture.Server.Modules.Users;

namespace Fracture.Server.Components.Pages;

public partial class GamePage
{
    private User _user = new();
    private readonly ObservableCollection<Item> _equipment = new();
    private readonly ObservableCollection<Item> _inventory = new();

    private Dictionary<string, object> _equipmentPopupParameters = null!;
    private Dictionary<string, object> _mapPopupParameters = null!;
    public static Map Map { get; set; }

    private PopupContainer _popup = null!;

    public static BackgroundImage BackgroundImage { get; set; } = new(string.Empty);

    private readonly MapDisplayData _mapDisplayData = new();

    private List<IPathfindingNode>? Path { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var username = await ProtectedSessionStore.GetAsync<string>("username");
        if (!username.Success)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        if (string.IsNullOrEmpty(username.Value))
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        var user = await UsersRepository.GetUserAsync(username.Value!);
        if (user is null)
        {
            user = new User { Username = username.Value! };
            await UsersRepository.AddUserAsync(user);
        }

        _user = user;

        _inventory.Clear();
        _equipment.Clear();
        foreach (var item in await ItemsRepository.GetItemsOfUserAsync(_user.Id))
        {
            _inventory.Add(item);
        }

        foreach (var item in _inventory.ToList().Where(i => i.IsEquipped))
        {
            _equipment.Add(item);
        }

        _equipmentPopupParameters = new Dictionary<string, object>
        {
            { "UserData", _user },
            { "Equipment", _equipment },
            { "Inventory", _inventory },
        };
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
