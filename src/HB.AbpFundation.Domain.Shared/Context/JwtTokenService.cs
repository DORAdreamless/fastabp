using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.ValueObject;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace HB.AbpFundation.Context
{
    public class JwtTokenService : IJwtTokenService,Volo.Abp.DependencyInjection.ITransientDependency
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionMultiplexer _redis;


        public JwtTokenService(IConfiguration configuration, ConnectionMultiplexer redis)
        {
            _configuration = configuration;
            _redis = redis;
        }

        public async Task<JWTToken> GenerateTokenAsync(Guid id, IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("AuthServer");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );
            JWTToken jwtToken = new JWTToken();
            jwtToken.access_token  = new JwtSecurityTokenHandler().WriteToken(token);
            jwtToken.expires_in = Convert.ToInt32(jwtSettings["ExpireMinutes"]);
            jwtToken.refresh_token = Guid.NewGuid().ToString("N");

           var db =  _redis.GetDatabase();
           await  db.HashSetAsync(RedisKeys.REFRESH_TOKEN, jwtToken.refresh_token, id.ToString());

            return jwtToken;
        }
    }
}
