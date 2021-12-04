using Pelatis.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pelatis.Data.Repositories
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsers();
        Task<IEnumerable<AppUser>> GetUsersWithDeleted();
        Task<AppUser> GetUser(int userId);
        Task<AppUser> GetUserByEmail(string email);
        Task<AppUser> GetUserWithDeleted(int userId);
        Task<AppUser> AddUser(AppUser user);
        Task<AppUser> UpdateUser(AppUser user);
        Task<bool> DeleteUser(int userId);

    }
}
