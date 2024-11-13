using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Items.Services;
using Microsoft.EntityFrameworkCore;

namespace Fracture.Server.Modules.Database
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly FractureDbContext _dbContext;

        public ItemsRepository(FractureDbContext dbContext, IItemGenerator itemGenerator)
        {
            _dbContext = dbContext;
        }

        public async Task<Item> AddItemAsync(Item item)
        {
            _dbContext.Items.Add(item);
            _dbContext.ItemStatistics.Add(item.Statistics);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<Item?> GetItemAsync(int id)
        {
            var item = await _dbContext
                .Items.Where(i => i.Id == id)
                .Include(i => i.Statistics)
                .FirstOrDefaultAsync();
            return item;
        }

        public async Task<ICollection<Item>> GetItemsOfUserAsync(int userId)
        {
            var items = await _dbContext
                .Items.Where(i => i.CreatedById == userId)
                .Include(i => i.Statistics)
                .ToListAsync();
            return items;
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}
