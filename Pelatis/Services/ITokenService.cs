

using Pelatis.Data.Entity;
using Pelatis.Dto;

namespace Pelatis.Services
{
    public interface ITokenService
    {
        void CreateToken(ref AppUserDto userDto,AppUser user);
    }
}
