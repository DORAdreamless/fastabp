using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HB.AbpFundation.DTOs.Common;

namespace HB.AbpFundation.Context
{
    public interface IJwtTokenService
    {
        Task<JWTToken> GenerateTokenAsync(Guid id, IEnumerable<Claim> claims);
    }
}
