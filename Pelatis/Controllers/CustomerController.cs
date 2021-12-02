using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pelatis.Data.Repositories;
using Pelatis.DTOs;
using Pelatis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Controllers
{
    public class CustomerController : Controller
    {
        protected readonly IBusinessRepository _businessRepository;
        protected readonly IAppUserRepository _appUserRepository;
        protected readonly ICustomerRepository _customerRepository;
        protected readonly ILogger<AccountsController> _logger;

        public CustomerController(IBusinessRepository businessRepository, IAppUserRepository appUserRepository,ICustomerRepository customerRepository, ILogger<AccountsController> logger)
        {
            _businessRepository = businessRepository;
            _appUserRepository = appUserRepository;
            _customerRepository = customerRepository;
            _logger = logger;

        }

        [HttpPost("add")]
        public async Task<ActionResult<CustomerDto>> AddBusiness(CustomerDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var business = await _businessRepository.GetBusiness(dealer.Business.Id);

                if (business == null)
                {
                    return BadRequest("Invalid Business");
                }

                var customer = await _customerRepository.GetCustomersByBusiness(business);

                if (customer != null)
                {
                    return BadRequest("Customer for Business Already Exist");
                }


                var newCustomer = new Customer
                {
                    Name = dealer.Name,
                    Email = dealer.Email,
                    Phone = dealer.Phone,
                    Business = business,
                };

                var createdCustomer = await _customerRepository.AddCustomer(newCustomer);


                return new CustomerDto(createdCustomer);
            }
            catch (Exception e)
            {
                _logger.LogError("Customer Add Error", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating Customer");
            }
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersForBusiness(int id)
        {
            try
            {
                var business = await _businessRepository.GetBusiness(id);
                if (business == null) return BadRequest("Business Not Found");

                var customers = await _customerRepository.GetCustomersByBusiness(business);

                return Ok(customers.Select(x => new CustomerDto(x)).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersForUser(int id)
        {
            try
            {
                var user = await _appUserRepository.GetUser(id);
                if (user == null) return BadRequest("User Not Found");

                var customers = await _customerRepository.GetCustomersByUser(user);

                return Ok(customers.Select(x => new CustomerDto(x)).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }


        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            try
            {
                var result = await _customerRepository.GetCustomer(id);
                if (result == null) return NotFound();

                return new CustomerDto(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult<CustomerDto>> Update(CustomerDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var customer = await _customerRepository.GetCustomer(dealer.Id);

                if (customer == null)
                {
                    return BadRequest("Customer Does not Exist");
                }

                var business = await _businessRepository.GetBusiness(dealer.Business.Id);

                if(business == null)
                {
                    return BadRequest("Invalid Business Id");
                }

                customer.Email = dealer.Email;
                customer.Name = dealer.Name;
                customer.Phone = dealer.Phone;
                customer.Business = dealer.Business;



                var updatedCustomer = await _customerRepository.UpdateCustomer(customer);
                return new CustomerDto(updatedCustomer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Customer");
            }
        }
    }
}
