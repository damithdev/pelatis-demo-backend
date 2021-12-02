using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pelatis.Data.Repositories;
using Pelatis.Dto;
using Pelatis.DTOs;
using Pelatis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Controllers
{
    public class BusinessController : BaseApiController
    {
        protected readonly IBusinessRepository _businessRepository;
        protected readonly IAppUserRepository _appUserRepository;
        protected readonly ILogger<AccountsController> _logger;

        public BusinessController(IBusinessRepository businessRepository,IAppUserRepository appUserRepository, ILogger<AccountsController> logger)
        {
            _businessRepository = businessRepository;
            _appUserRepository = appUserRepository;
            _logger = logger;

        }

        [HttpPost("add")]
        public async Task<ActionResult<BusinessDto>> AddBusiness(BusinessDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = await _appUserRepository.GetUser(dealer.AppUser.Id);

                if (user == null)
                {
                    return BadRequest("Invalid User");
                }

                var business = await _businessRepository.GetBusinessesByUser(user);

                if(business != null)
                {
                    return BadRequest("Business for the user Already Exist");
                }


                var newBusiness = new Business
                {
                    CompanyName = dealer.CompanyName,
                    TypeOfBusiness = dealer.TypeOfBusiness,
                    Country = dealer.Country,
                    Currency = dealer.Currency,
                    AppUser = user
                };

                var createdBusiness = await _businessRepository.AddBusiness(newBusiness);

                if(user.DefaultBusiness == 0)
                {
                    user.DefaultBusiness = createdBusiness.Id;
                    await _appUserRepository.UpdateUser(user);
                }

                return new BusinessDto(createdBusiness);
            }
            catch (Exception e)
            {
                _logger.LogError("Business Register Error", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating Business");
            }
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<IEnumerable<BusinessDto>>> GetBusinessesForUser(int id)
        {
            try
            {
                var user = await _appUserRepository.GetUser(id);
                if (user == null) return BadRequest("User Not Found");

                var businesses = await _businessRepository.GetBusinessesByUser(user);

                return Ok(businesses.Select(x => new BusinessDto(x)).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }


        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<BusinessDto>> GetBusiness(int id)
        {
            try
            {
                var result = await _businessRepository.GetBusiness(id);
                if (result == null) return NotFound();

                return new BusinessDto(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult<BusinessDto>> Update(BusinessDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var business = await _businessRepository.GetBusiness(dealer.Id);

                if (business == null)
                {
                    return BadRequest("Business Does not Exist");
                }

                business.CompanyName = dealer.CompanyName;
                business.TypeOfBusiness = dealer.TypeOfBusiness;
                business.Country = dealer.Country;
                business.Currency = dealer.Currency;


                var updatedBusiness = await _businessRepository.UpdateBusiness(business);
                return new BusinessDto(updatedBusiness);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Business");
            }
        }


        [HttpPost("switch_default_business")]
        public async Task<ActionResult<AppUserDto>> SwitchDefaultBusiness(AppUserDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = await _appUserRepository.GetUserByEmail(dealer.Email);//TODO get user from auth

                if (user == null)
                {
                    return BadRequest("User Does not Exist");
                }

                var business = await _businessRepository.GetBusiness(dealer.DefaultBusinessId);

                if (business == null)
                {
                    return BadRequest("Business Does not Exist");
                }

                user.DefaultBusiness = dealer.DefaultBusinessId;


                var updatedUser = await _appUserRepository.UpdateUser(user);
                return new AppUserDto(updatedUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating User");
            }
        }
    }
}
