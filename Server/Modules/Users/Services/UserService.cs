using System.Collections.ObjectModel;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users.Models;
using static System.Reflection.Metadata.BlobBuilder;

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

    /// <summary>
    /// How many items can be equipped at the same time. Rings don't count towards this limit, as you can equip multiple rings.
    /// </summary>
    public int EquipmentSlots { get; } = 6;

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

    public void Equip(Item item)
    {
        if (Equipment.Count < EquipmentSlots)
        {
            if (item.Type.Equals(ItemType.Ring))
            {
                item.IsEquipped = true;
                _itemsRepository.UpdateItemAsync(item);
                Equipment.Add(item);
                return;
            }

            foreach (var equipped in Equipment)
            {
                if (equipped.Type.Equals(item.Type))
                {
                    return;
                }
            }

            item.IsEquipped = true;
            _itemsRepository.UpdateItemAsync(item);
            Equipment.Add(item);
        }
    }

    public void Unequip(Item item)
    {
        item.IsEquipped = false;
        _itemsRepository.UpdateItemAsync(item);
        Equipment.Remove(item);
    }

    public bool IsEquipped(Item item)
    {
        return Equipment.Contains(item);
    }
}
