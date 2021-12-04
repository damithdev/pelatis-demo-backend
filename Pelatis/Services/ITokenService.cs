

using Pelatis.Data.Entity;

namespace Pelatis.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
