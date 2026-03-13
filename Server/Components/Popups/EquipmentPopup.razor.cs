using System.Collections.ObjectModel;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users.Models;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Components.Popups;

public partial class EquipmentPopup
{
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
        UserService.Equip(item);
    }

    public void Unequip(Item item)
    {
        UserService.Unequip(item);
    }
}
