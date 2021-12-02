using Pelatis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Data.Repositories
{
    public interface IBusinessRepository
    {
        Task<Business> AddBusiness(Business business);
        Task<IEnumerable<Business>> GetBusinesses();
        Task<IEnumerable<Business>> GetBusinessesByUser(AppUser user);
        Task<Business> GetBusiness(int businessId);
        Task<Business> UpdateBusiness(Business business);
        Task<bool> DeleteBusiness(int businessId);
    }
}
