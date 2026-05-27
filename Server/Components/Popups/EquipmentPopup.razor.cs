using System.Collections.ObjectModel;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users.Models;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Components.Popups;

public partial class EquipmentPopup
{
    protected override async Task OnInitializedAsync() { }

    public void Equip(Item item)
    {
        UserService.Equip(item);
    }

    public void Unequip(Item item)
    {
        UserService.Unequip(item);
    }

    public string SelectedType { get; set; } = "All";
}
