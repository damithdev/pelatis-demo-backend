
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pelatis.Config.Filters;
using Pelatis.Data.Entity;
using Pelatis.Data.Repositories;
using Pelatis.Dto;
using System;
using System.Threading.Tasks;

namespace Pelatis.Controllers
{
    [Authorize]
    public class AppUsersController : BaseApiController
    {
        private readonly AppUser _authenticatedUser;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ILogger<AccountsController> _logger;

        public AppUsersController(IAppUserRepository appUserRepository, ILogger<AccountsController> logger)
        {
            _appUserRepository = appUserRepository;
            _logger = logger;
        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers()
        //{
        //    try
        //    {
        //        var users = await _appUserRepository.GetUsers();
        //        return Ok(users.Select(x => new AppUserDto(x)).ToList());
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
        //    }
        //}

        [UserAuthorizeAttribute]
        [HttpGet("get")]
        public ActionResult<AppUserDto> GetUser()
        {
            try
            {
                return new AppUserDto((AppUser)HttpContext.Items["User"]);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data");
            }
        }

        [UserAuthorizeAttribute]
        [HttpPost("edit")]
        public async Task<ActionResult<AppUserDto>> Update(AppUserDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = (AppUser)HttpContext.Items["User"];

                if (user == null)
                {
                    return BadRequest("User Does not Exist");
                }

                user.FirstName = dealer.FirstName;
                user.LastName = dealer.LastName;

                if (user.Email != dealer.Email)
                {
                    var userWithEmail = await _appUserRepository.GetUserByEmail(dealer.Email);
                    if (userWithEmail != null && userWithEmail.Id != user.Id)
                    {
                        return BadRequest("Email Address Already Occupied");
                    }
                }

                var updatedUser = await _appUserRepository.UpdateUser(user);
                return new AppUserDto(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogError("User Update Ex:", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating User");
            }
        }

        [UserAuthorizeAttribute]
        [HttpPost("onboard")]
        public async Task<ActionResult<AppUserDto>> OnBoard(AppUserDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = (AppUser)HttpContext.Items["User"];

                if (user == null)
                {
                    return BadRequest("User Does not Exist");
                }

                user.FirstName = dealer.FirstName;
                user.LastName = dealer.LastName;

                var updatedUser = await _appUserRepository.UpdateUser(user);
                return new AppUserDto(user);
            }
            catch (Exception e)
            {
                _logger.LogError("User Onboard Ex:", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Onboarding User");
            }
        }



    }
}
