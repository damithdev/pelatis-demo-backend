using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pelatis.Data.Entity;
using Pelatis.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pelatis.Services
{
    public class TokenService : ITokenService
    {
        protected readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public void CreateToken(ref AppUserDto dto,AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id",user.Id.ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var exp = DateTime.Now.AddDays(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = exp,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            dto.Expiry = tokenDescriptor.Expires;
            dto.Token = tokenHandler.WriteToken(token);
        }

    }
}
