using Fracture.Server.Modules.Items.Models;

namespace Fracture.Server.Modules.Database;

public interface IItemsRepository
{
    public Task<Item> AddItemAsync(Item item);
    public Task<Item?> GetItemAsync(int id);
    public Task<ICollection<Item>> GetItemsOfUserAsync(int userId);
    public Task<Item> UpdateItemAsync(Item item);
}
