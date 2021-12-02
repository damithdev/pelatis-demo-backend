using Microsoft.EntityFrameworkCore;
using Pelatis.Entities;
using Pelatis.Workers.Errors;
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
            user.UpdatedDate = DateTime.Now;
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
                int state = await _context.SaveChangesAsync();

                if (state == 1) return true;

            }

            return false;
        }

        public async Task<AppUser> GetUser(int userId)
        {
            return await _context.AppUsers.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _context.AppUsers.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _context.AppUsers.Where(u => u.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersWithDeleted()
        {
            return await _context.AppUsers.ToListAsync();
        }

        public async Task<AppUser> GetUserWithDeleted(int userId)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser> UpdateUser(AppUser user)
        {
            var result = await _context.AppUsers.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Id == user.Id);

            if (result != null)
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.Email = user.Email;
                result.CreatedDate = user.CreatedDate;
                result.UpdatedDate = DateTime.Now;
                result.Secret = user.Secret;
                result.Salt = user.Salt;

                await _context.SaveChangesAsync();
            }

            return result;

        }
    }
}
