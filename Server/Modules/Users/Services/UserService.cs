using System.Collections.ObjectModel;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users.Models;

namespace Fracture.Server.Modules.Users.Services;

public class UserService(IItemsRepository _itemsRepository)
{
    public User? User { get; private set; }

    /// <summary>
    /// Inventory is a list of all items user has, while Equipment is a list of currently equipped items.
    /// An item will be in both lists if it's equipped, but if it's not equipped, it will only be in Inventory.
    /// </summary>
    public ObservableCollection<Item> Inventory { get; } = new();

    /// <summary>
    /// Equipment is a list of currently equipped items.
    /// </summary>
    public ObservableCollection<Item> Equipment { get; } = new();

    public async Task LoadUserAsync(User user)
    {
        User = user;
        Inventory.Clear();
        Equipment.Clear();

        foreach (var item in await _itemsRepository.GetItemsOfUserAsync(user.Id))
        {
            Inventory.Add(item);
            if (item.IsEquipped)
                Equipment.Add(item);
        }
    }
}
