using Fracture.Server.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace Fracture.Server.Modules.Database
{
    public class UsersRepository : IUsersRepository
    {
        private readonly FractureDbContext _dbContext;

        public UsersRepository(FractureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == id)
                .Include(u => u.Items)
                .ThenInclude(i => i.Statistics)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<User?> GetUserAsync(string username)
        {
            var user = await _dbContext.Users
                .Where(u => u.Username == username)
                .Include(u => u.Items)
                .ThenInclude(i => i.Statistics)
                .FirstOrDefaultAsync();
            return user;
        }
    }
}
