using Microsoft.EntityFrameworkCore;
using Pelatis.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Data.Repositories
{
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        private readonly DataContext _context;

        public CustomerRepositoryImpl(DataContext context)
        {
            _context = context;
        }
        public async Task<Customer> AddCustomer(Customer customer)
        {
            customer.CreatedDate = DateTime.Now;
            customer.UpdatedDate = DateTime.Now;

            var result = await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            var result = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            if (result != null)
            {
                _context.Customers.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch { return null; }
        }

        public async Task<Customer> GetCustomerOfUser(AppUser user, int customerId)
        {
            try
            {
                return await _context.Customers.Where(c => c.Business.AppUser == user).FirstOrDefaultAsync(c => c.Id == customerId);

            }
            catch { return null; }
        }

        public async Task<Customer> GetCustomerOfBusiness(Business business, int customerId)
        {
            try
            {
                return await _context.Customers.Where(c => c.Business == business).FirstOrDefaultAsync(c => c.Id == customerId);

            }
            catch { return null; }
        }

        public async Task<Customer> GetCustomerOfBusinessByEmail(Business business, string email)
        {
            try
            {
                return await _context.Customers.Where(c => c.Business == business).FirstOrDefaultAsync(c => c.Email == email);

            }
            catch { return null; }
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            try
            {
                return await _context.Customers.ToListAsync();

            }
            catch { return null; }
        }

        public async Task<IEnumerable<Customer>> GetCustomersByBusiness(Business business)
        {
            try
            {
                return await _context.Customers.Where(c => c.Business == business).ToListAsync();

            }
            catch { return null; }
        }

        public async Task<IEnumerable<Customer>> GetCustomersByUser(AppUser user)
        {
            try
            {
                return await _context.Customers.Where(c => c.Business.AppUser == user).ToListAsync();
            }
            catch { return null; }
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var result = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
            if (result != null)
            {
                result.Name = customer.Name;
                result.Email = customer.Email;
                result.Phone = customer.Phone;
                result.UpdatedDate = DateTime.Now;
                if (customer.Business != null)
                {
                    result.Business = customer.Business;
                }
                await _context.SaveChangesAsync();
            }
            return result;
        }


    }
}
