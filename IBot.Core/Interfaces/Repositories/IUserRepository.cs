using IBot.Core.Entities.Users;

namespace IBot.Core.Interfaces.Repositories;

public interface IUserRepository:IRepository<User>
{
    public Task<User?> GetByTelegramIdAsync(long id);
}