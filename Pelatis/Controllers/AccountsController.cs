using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pelatis.Data.Entity;
using Pelatis.Data.Repositories;
using Pelatis.Dto;
using Pelatis.DTOs;
using Pelatis.Helpers;
using Pelatis.Helpers.Utilities;
using Pelatis.Services;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Pelatis.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly ILogger<AccountsController> _logger;

        private readonly IAppUserRepository _appUserRepository;

        private readonly ITokenService _tokenService;
        public AccountsController(IAppUserRepository appUserRepository, ITokenService tokenService, ILogger<AccountsController> logger)
        {
            _appUserRepository = appUserRepository;
            _tokenService = tokenService;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<ActionResult<AppUserDto>> Register(LoginDto dealer)
        {
            try
            {
                if (dealer == null) return BadRequest();

                var user = await _appUserRepository.GetUserByEmail(dealer.Email);

                if (user != null)
                {
                    return BadRequest("User With Email Alerady Exist");
                }


                using var hmac = new HMACSHA512();
                byte[] salt = new byte[] { };
                byte[] secret = new HMACUtility().ComputeHash(ref salt, dealer.Password);

                var newUser = new AppUser
                {
                    Email = dealer.Email.ToLower(),
                    Salt = salt,
                    Secret = secret,
                };


                var createdUser = await _appUserRepository.AddUser(newUser);
                var userDto = new AppUserDto(createdUser);
                _tokenService.CreateToken(ref userDto,createdUser);
                return userDto;
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

                var user = await _appUserRepository.GetUserByEmail(dealer.Email);

                if (user == null)
                {
                    return BadRequest();
                }

                var computedHash = new HMACUtility().ComputeHashWithSalt(user.Salt, dealer.Password);

                if (computedHash.Length == user.Secret.Length)
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
                var userDto = new AppUserDto(user);
                _tokenService.CreateToken(ref userDto,user);
                return userDto;
            }
            catch (Exception e)
            {
                _logger.LogError("User Login Error", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating User");
            }
        }

    }
}
