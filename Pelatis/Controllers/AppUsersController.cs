
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

namespace Pelatis.Controllers
{
    public class AppUsersController : BaseApiController
    {

        protected readonly IAppUserRepository _appUserRepository;
        public AppUsersController(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            try
            {
                return Ok(await _appUserRepository.GetUsers());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            try
            {
                var result = await _appUserRepository.GetUser(id);
                if (result == null) return NotFound();

                return result;
            }catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult<AppUser>> Update(AppUserDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = _appUserRepository.GetUserByEmail(dealer.Email);

                if (user == null)
                {
                    return BadRequest("User Does not Exist");
                }

                var newUser = new AppUser{
                    FirstName = dealer.FirstName,
                    LastName = dealer.LastName,
                    Email = dealer.Email,
                };


                return await _appUserRepository.UpdateUser(newUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating User");
            }
        }

    }
}
