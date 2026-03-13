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
            item.CreatedBy = UserService.User!;
            item.CreatedById = UserService.User!.Id;

            await ItemsRepository.AddItemAsync(item);
            UserService.Inventory.Add(item);
        }
    }

    public void Equip(Item item)
    {
        if (UserService.Equipment.Count < slots)
        {
            if (item.Type.Equals(ItemType.Ring))
            {
                item.IsEquipped = true;
                ItemsRepository.UpdateItemAsync(item);
                UserService.Equipment.Add(item);
                return;
            }

            foreach (var equipped in UserService.Equipment)
            {
                if (equipped.Type.Equals(item.Type))
                {
                    return;
                }
            }

            item.IsEquipped = true;
            ItemsRepository.UpdateItemAsync(item);
            UserService.Equipment.Add(item);
        }
    }

    public void Unequip(Item item)
    {
        item.IsEquipped = false;
        ItemsRepository.UpdateItemAsync(item);
        UserService.Equipment.Remove(item);
    }

    public bool IsEquipped(Item item)
    {
        return UserService.Equipment.Contains(item);
    }
}
