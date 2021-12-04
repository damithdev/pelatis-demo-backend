using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pelatis.Data.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pelatis.Config.Extensions
{
    public class JWTContextInjector
    {
        private readonly RequestDelegate _next;
        protected readonly SymmetricSecurityKey _key;

        public JWTContextInjector(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task Invoke(HttpContext context, IAppUserRepository userRepository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null) await AtachUserToContext(context, userRepository, token);

            await _next(context);

        }

        private async Task AtachUserToContext(HttpContext context, IAppUserRepository userRepository, string token)
        {
            try
            {
                if (!context.Request.Headers.ContainsKey("SkipJWTContextInjector"))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = _key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                    context.Items["User"] = await userRepository.GetUser(userId);
                }

            }
            catch
            {
                // Do nothing
            }
        }
    }
}
