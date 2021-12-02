using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Pelatis.Data;
using Pelatis.Data.Repositories;
using Pelatis.Dto;
using Pelatis.DTOs;
using Pelatis.Entities;
using Pelatis.Helpers;
using Pelatis.Helpers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pelatis.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly ILogger<AccountsController> _logger;

        private readonly IAppUserRepository _appUserRepository;
        public AccountsController(IAppUserRepository appUserRepository, ILogger<AccountsController> logger)
        {
            _appUserRepository = appUserRepository;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<ActionResult<AppUserDto>> Register(AppUserDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = await _appUserRepository.GetUserByEmail(dealer.Email);

                if(user != null)
                {
                    return BadRequest("User With Email Alerady Exist");
                }


                using var hmac = new HMACSHA512();
                byte[] salt = new byte[] { };
                byte[] secret = new HMACUtility().ComputeHash(ref salt,dealer.Password);
               
                var newUser = new AppUser {
                    FirstName = dealer.FirstName,
                    LastName = dealer.LastName,
                    Email = dealer.Email.ToLower(),
                    Salt = salt,
                    Secret = secret,
                };


                var createdUser = await _appUserRepository.AddUser(newUser);
                // TODO Get New User Objest
                return new AppUserDto(createdUser);
            }
            catch (Exception e)
            {
                _logger.LogError("User Register Error", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating User");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUserDto>> Login(LoginDto dealer)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(dealer.Email) || String.IsNullOrWhiteSpace(dealer.Password)) return BadRequest();

                var user = await _appUserRepository.GetUserByEmail(dealer.Password);

                if (user != null)
                {
                    return BadRequest();
                }

                var computedHash = new HMACUtility().ComputeHashWithSalt(user.Salt,dealer.Password);

                if(computedHash.Length == user.Secret.Length)
                {
                    for (int i = 0; i < user.Secret.Length; i++)
                    {
                        if (computedHash[i] != user.Secret[i]) return Unauthorized(StaticEntry.InvalidCreds);
                    }
                }
                else
                {
                    return Unauthorized(StaticEntry.InvalidCreds);
                }
                // TODO Get Updated User Objest

                return new AppUserDto(user);

            }
            catch (Exception e)
            {
                _logger.LogError("User Login Error", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating User");
            }
        }

    }
}
