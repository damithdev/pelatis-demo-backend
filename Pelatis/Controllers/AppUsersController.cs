
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pelatis.Data;
using Pelatis.Data.Repositories;
using Pelatis.Entities;
using System;
using Pelatis.Dto;
using Microsoft.Extensions.Logging;

namespace Pelatis.Controllers
{
    public class AppUsersController : BaseApiController
    {

        protected readonly IAppUserRepository _appUserRepository;
        private readonly ILogger<AccountsController> _logger;

        public AppUsersController(IAppUserRepository appUserRepository, ILogger<AccountsController> logger)
        {
            _appUserRepository = appUserRepository;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers()
        {
            try
            {
                var users = await _appUserRepository.GetUsers();
                return Ok(users.Select(x => new AppUserDto(x)).ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUserDto>> GetUser(int id)
        {
            try
            {
                var result = await _appUserRepository.GetUser(id);
                if (result == null) return NotFound();

                return new AppUserDto(result);
            }catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }


        [HttpPost("edit")]
        public async Task<ActionResult<AppUserDto>> Update(AppUserDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = await _appUserRepository.GetUserByEmail(dealer.Email);

                if (user == null)
                {
                    return BadRequest("User Does not Exist");
                }

                user.FirstName = dealer.FirstName;
                user.LastName = dealer.LastName;
                user.Email = dealer.Email;


                var updatedUser =  await _appUserRepository.UpdateUser(user);
                return new AppUserDto(updatedUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating User");
            }
        }



    }
}
