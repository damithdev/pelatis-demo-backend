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
    public class BusinessController : BaseApiController
    {
        protected readonly IBusinessRepository _businessRepository;
        protected readonly IAppUserRepository _appUserRepository;
        protected readonly ILogger<AccountsController> _logger;

        public BusinessController(IBusinessRepository businessRepository, IAppUserRepository appUserRepository, ILogger<AccountsController> logger)
        {
            _businessRepository = businessRepository;
            _appUserRepository = appUserRepository;
            _logger = logger;

        }

        [UserAuthorizeAttribute]
        [HttpPost("add")]
        public async Task<ActionResult<BusinessDto>> AddBusiness(BusinessDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = (AppUser)HttpContext.Items["User"];

                if (user == null)
                {
                    return BadRequest("Invalid User");
                }

                var business = await _businessRepository.GetBusinessByUserAndName(user, dealer.CompanyName);

                if (business != null)
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


                user.DefaultBusiness = createdBusiness.Id;
                await _appUserRepository.UpdateUser(user);


                return new BusinessDto(createdBusiness);
            }
            catch (Exception e)
            {
                _logger.LogError("Business Register Error", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating Business");
            }
        }

        [UserAuthorizeAttribute]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<BusinessDto>>> GetBusinessesForUser()
        {
            try
            {
                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");


                var businesses = await _businessRepository.GetBusinessesByUser(user);

                return Ok(businesses.Select(x => new BusinessDto(x)).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [UserAuthorizeAttribute]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<BusinessDto>> GetBusiness(int id)
        {
            try
            {
                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");

                var result = await _businessRepository.GetBusinessByUserAndId(user, id);
                if (result == null) return NotFound();

                return new BusinessDto(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [UserAuthorizeAttribute]
        [HttpPost("edit")]
        public async Task<ActionResult<BusinessDto>> Update(BusinessDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();
                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");

                var business = await _businessRepository.GetBusinessByUserAndId(user, dealer.Id);

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

        [UserAuthorizeAttribute]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<BusinessDto>> SwitchDefaultBusiness(int id)
        {
            try
            {
                if (id == 0) return BadRequest();

                var user = (AppUser)HttpContext.Items["User"];
                if (user == null) return BadRequest("Invalid User");

                var business = await _businessRepository.GetBusinessByUserAndId(user, id);

                if (business == null)
                {
                    return BadRequest("Business Does not Exist");
                }

                user.DefaultBusiness = business.Id;


                var updatedUser = await _appUserRepository.UpdateUser(user);
                var defaultBusiness = await _businessRepository.GetBusinessByUserAndId(updatedUser, updatedUser.DefaultBusiness);

                return new BusinessDto(defaultBusiness);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating User");
            }
        }
    }
}
