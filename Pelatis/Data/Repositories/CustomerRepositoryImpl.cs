using Microsoft.EntityFrameworkCore;
using Pelatis.Entities;
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
            if(result != null)
            {
                _context.Customers.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersByBusiness(Business business)
        {
            return await _context.Customers.Where(c => c.Business == business).ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersByUser(AppUser user)
        {
            return await _context.Customers.Where(c => c.Business.AppUser == user).ToListAsync();
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var result = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
            if(result != null)
            {
                result.Name = customer.Name;
                result.Email = customer.Email;
                result.Phone = customer.Phone;
                result.UpdatedDate = DateTime.Now;
                if(customer.Business != null)
                {
                    result.Business = customer.Business;
                }
                await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
