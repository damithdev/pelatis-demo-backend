using Pelatis.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pelatis.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<IEnumerable<Customer>> GetCustomers();
        Task<IEnumerable<Customer>> GetCustomersByUser(AppUser user);
        Task<IEnumerable<Customer>> GetCustomersByBusiness(Business business);
        Task<Customer> GetCustomer(int id);
        Task<Customer> GetCustomerOfUser(AppUser user,int customerId);
        Task<Customer> GetCustomerOfBusiness(Business business,int customerId);
        Task<Customer> GetCustomerOfBusinessByEmail(Business business,string email);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int customerId);
    }
}
