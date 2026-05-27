namespace Fracture.Server.Components.Popups;

public partial class SettingsPopup
{
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
}
