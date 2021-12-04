using Microsoft.EntityFrameworkCore;
using Pelatis.Data.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Data.Repositories
{
    public class AppUserRepositoryImpl : IAppUserRepository
    {
        private readonly DataContext _context;

        public AppUserRepositoryImpl(DataContext context)
        {
            _context = context;
        }


        public async Task<AppUser> AddUser(AppUser user)
        {
            user.CreatedDate = DateTime.Now;
            //user.UpdatedDate = DateTime.Now;
            var result = await _context.AppUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var result = await _context.AppUsers.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Id == userId);

            if (result != null)
            {
                result.IsDeleted = true;
                await _context.SaveChangesAsync();

                return true;

            }

            return false;
        }

        public async Task<AppUser> GetUser(int userId)
        {
            try
            {
                return await _context.AppUsers.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Id == userId);

            }
            catch
            {
                return null;
            }
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            try
            {
                return await _context.AppUsers.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Email == email);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            try
            {
                return await _context.AppUsers.Where(u => u.IsDeleted == false).ToListAsync();

            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<AppUser>> GetUsersWithDeleted()
        {
            try
            {
                return await _context.AppUsers.ToListAsync();

            }
            catch
            {
                return null;
            }
        }

        public async Task<AppUser> GetUserWithDeleted(int userId)
        {
            try
            {
                return await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<AppUser> UpdateUser(AppUser user)
        {

                var result = await _context.AppUsers.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Id == user.Id);

                if (result != null)
                {
                    result.FirstName = user.FirstName;
                    result.LastName = user.LastName;
                    result.Email = user.Email;
                    result.UpdatedDate = DateTime.Now;
                    if (user.DefaultBusiness > 0)
                    {
                        result.DefaultBusiness = user.DefaultBusiness;
                    }

                    await _context.SaveChangesAsync();
                }

                return result;
            }
            
    }
}
