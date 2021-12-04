using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pelatis.Config.Filters;
using Pelatis.Data.Entity;
using Pelatis.Data.Repositories;
using Pelatis.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Controllers
{
    [Authorize]
    public class CustomerController : BaseApiController
    {
        protected readonly IBusinessRepository _businessRepository;
        protected readonly IAppUserRepository _appUserRepository;
        protected readonly ICustomerRepository _customerRepository;
        protected readonly ILogger<AccountsController> _logger;

        public CustomerController(IBusinessRepository businessRepository, IAppUserRepository appUserRepository, ICustomerRepository customerRepository, ILogger<AccountsController> logger)
        {
            _businessRepository = businessRepository;
            _appUserRepository = appUserRepository;
            _customerRepository = customerRepository;
            _logger = logger;

        }

        [UserAuthorizeAttribute]
        [HttpPost("add")]
        public async Task<ActionResult<CustomerDto>> AddCustomer(CustomerDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");

                var business = await _businessRepository.GetBusinessByUserAndId(user, dealer.BusinessId);

                if (business == null)
                {
                    return BadRequest("Invalid Business");
                }

                var customer = await _customerRepository.GetCustomerOfBusinessByEmail(business, dealer.Email);

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

        [UserAuthorizeAttribute]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersForBusiness(int id)
        {
            try
            {
                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");
                var business = await _businessRepository.GetBusinessByUserAndId(user, id);
                if (business == null) return BadRequest("Business Not Found");

                var customers = await _customerRepository.GetCustomersByBusiness(business);

                return Ok(customers.Select(x => new CustomerDto(x)).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [UserAuthorizeAttribute]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersForUser(int id)
        {
            try
            {
                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");

                var customers = await _customerRepository.GetCustomersByUser(user);

                return Ok(customers.Select(x => new CustomerDto(x)).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }


        [UserAuthorizeAttribute]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            try
            {
                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");

                var result = await _customerRepository.GetCustomerOfUser(user, id);
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

                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");


                var business = await _businessRepository.GetBusinessByUserAndId(user, dealer.BusinessId);

                if (business == null)
                {
                    return BadRequest("Invalid Business Id");
                }

                var customer = await _customerRepository.GetCustomerOfBusiness(business, dealer.Id);

                if (customer == null)
                {
                    return BadRequest("Customer Does not Exist");
                }




                customer.Email = dealer.Email;
                customer.Name = dealer.Name;
                customer.Phone = dealer.Phone;


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
