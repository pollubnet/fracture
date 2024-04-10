using Fracture.Server.Modules.Users;

namespace Fracture.Server.Modules.Database
{
    public interface IUsersRepository
    {
        public Task<User> AddUserAsync(User user);
        public Task<User?> GetUserAsync(int id);
        public Task<User?> GetUserAsync(string username);
    }
}
