using Microsoft.EntityFrameworkCore;
using Pelatis.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Data.Repositories
{
    public class BusinessRepositoryImpl : IBusinessRepository
    {
        private readonly DataContext _context;

        public BusinessRepositoryImpl(DataContext context)
        {
            _context = context;
        }
        public async Task<Business> AddBusiness(Business business)
        {
            business.CreatedDate = DateTime.Now;

            var result = await _context.Businesses.AddAsync(business);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteBusiness(int businessId)
        {
            var result = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == businessId);
            if (result != null)
            {
                _context.Businesses.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Business> GetBusinessByUserAndId(AppUser appUser, int businessId)
        {
            try
            {
                return await _context.Businesses.Where(b => b.AppUser == appUser).FirstOrDefaultAsync(b => b.Id == businessId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Business>> GetBusinesses()
        {
            try
            {
                return await _context.Businesses.ToListAsync();
            }
            catch { return null; }
        }

        public async Task<IEnumerable<Business>> GetBusinessesByUser(AppUser user)
        {
            try
            {
                return await _context.Businesses.Where(b => b.AppUser == user).ToListAsync();

            }
            catch
            {
                return null;
            }
        }

        public async Task<Business> GetBusinessByUserAndName(AppUser user, String companyName)
        {
            try
            {
                return await _context.Businesses.Where(b => b.AppUser == user).FirstOrDefaultAsync(b => b.CompanyName == companyName);
            }
            catch { return null; }
        }

        public async Task<Business> UpdateBusiness(Business business)
        {

            var result = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == business.Id);
            if (result != null)
            {
                result.CompanyName = business.CompanyName;
                result.TypeOfBusiness = business.TypeOfBusiness;
                result.Country = business.Country;
                result.Currency = business.Currency;
                result.UpdatedDate = DateTime.Now;
                if (business.AppUser != null)
                {
                    result.AppUser = business.AppUser;
                }

                await _context.SaveChangesAsync();

            }
            return result;


        }
    }
}
