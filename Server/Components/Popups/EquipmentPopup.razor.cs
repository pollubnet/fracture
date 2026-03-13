using System.Collections.ObjectModel;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users.Models;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Components.Popups;

public partial class EquipmentPopup
{
    private const int slots = 6;

    protected override async Task OnInitializedAsync() { }

    public async Task GenerateNewItem()
    {
        var item = await ItemGenerator.Generate();

        if (item is not null)
        {
            item.CreatedBy = _userInventoryService.User!;
            item.CreatedById = _userInventoryService.User!.Id;

            await ItemsRepository.AddItemAsync(item);
            _userInventoryService.Inventory.Add(item);
        }
    }

    public void Equip(Item item)
    {
        if (_userInventoryService.Equipment.Count < slots)
        {
            if (item.Type.Equals(ItemType.Ring))
            {
                item.IsEquipped = true;
                ItemsRepository.UpdateItemAsync(item);
                _userInventoryService.Equipment.Add(item);
                return;
            }

            foreach (var equipped in _userInventoryService.Equipment)
            {
                if (equipped.Type.Equals(item.Type))
                {
                    return;
                }
            }

            item.IsEquipped = true;
            ItemsRepository.UpdateItemAsync(item);
            _userInventoryService.Equipment.Add(item);
        }
    }

    public void Unequip(Item item)
    {
        item.IsEquipped = false;
        ItemsRepository.UpdateItemAsync(item);
        _userInventoryService.Equipment.Remove(item);
    }

    public bool IsEquipped(Item item)
    {
        return _userInventoryService.Equipment.Contains(item);
    }
}
