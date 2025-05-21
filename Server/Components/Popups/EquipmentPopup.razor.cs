using System.Collections.ObjectModel;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Components.Popups;

public partial class EquipmentPopup
{
    [Parameter]
    public required ObservableCollection<Item> Inventory { get; set; }

    [Parameter]
    public required ObservableCollection<Item> Equipment { get; set; }

    [Parameter]
    public required User UserData { get; set; }

    private const int slots = 6;

    protected override async Task OnInitializedAsync()
    {
        Inventory.Clear();
        Equipment.Clear();
        foreach (var item in await ItemsRepository.GetItemsOfUserAsync(UserData.Id))
        {
            Inventory.Add(item);
        }

        foreach (var item in Inventory.ToList().Where(i => i.IsEquipped))
        {
            Equipment.Add(item);
        }

        Logger.LogInformation($"Inventory initialized for user: {UserData.Username}");
    }

    public async void Refresh()
    {
        Logger.LogInformation($"Refreshing! New username: {UserData.Username}");
        Inventory.Clear();
        Equipment.Clear();
        foreach (var item in await ItemsRepository.GetItemsOfUserAsync(UserData.Id))
        {
            Inventory.Add(item);
        }

        foreach (var item in Inventory.ToList().Where(i => i.IsEquipped))
        {
            Equipment.Add(item);
        }

        await OnInitializedAsync();
    }

    public async Task GenerateNewItem()
    {
        var item = await ItemGenerator.Generate();

        if (item is not null)
        {
            item.CreatedBy = UserData;
            item.CreatedById = UserData.Id;
            await ItemsRepository.AddItemAsync(item);
            Inventory.Add(item);
        }
    }

    public void Equip(Item item)
    {
        if (Equipment.Count < slots)
        {
            if (item.Type.Equals(ItemType.Ring))
            {
                item.IsEquipped = true;
                ItemsRepository.UpdateItemAsync(item);
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
            ItemsRepository.UpdateItemAsync(item);
            Equipment.Add(item);
        }
    }

    public void Unequip(Item item)
    {
        item.IsEquipped = false;
        ItemsRepository.UpdateItemAsync(item);
        Equipment.Remove(item);
    }

    public bool IsEquipped(Item item)
    {
        return Equipment.Contains(item);
    }
}
