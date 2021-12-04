using Pelatis.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pelatis.Data.Repositories
{
    public interface IBusinessRepository
    {
        Task<Business> AddBusiness(Business business);
        Task<IEnumerable<Business>> GetBusinesses();
        Task<IEnumerable<Business>> GetBusinessesByUser(AppUser user);
        Task<Business> GetBusinessByUserAndName(AppUser user, String companyName);
        Task<Business> GetBusinessByUserAndId(AppUser user, int businessId);
        Task<Business> UpdateBusiness(Business business);
        Task<bool> DeleteBusiness(int businessId);
    }
}
