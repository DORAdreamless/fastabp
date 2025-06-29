using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using HB.AbpFundation.Helpers;
using HB.AbpFundation.ValueObject;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using Volo.Abp.Domain.Entities;

namespace HB.AbpFundation.Context
{
    public class ContextUser
    {
     

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }


        public Guid? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public List<string> RoleNames { get; set; }

        public string RoleDescriptions { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();

        public string Introduction { get; set; }

        public string Name { get; set; }

    }

    public class ContextService : IContextService, Volo.Abp.DependencyInjection.IScopedDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ConnectionMultiplexer _redis;

        public ContextService(IHttpContextAccessor httpContextAccessor, ConnectionMultiplexer redis)
        {
            _httpContextAccessor = httpContextAccessor;
            _redis = redis;
        }

        public HttpContext HttpContext
        {
            get
            {
                if (_httpContextAccessor == null)
                {
                    return null;
                }
                return _httpContextAccessor.HttpContext;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (this.HttpContext == null)
                {
                    return false;
                }
                return HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public Guid? UserId
        {
            get
            {
                if (IsAuthenticated)
                {
                    var valueStr = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
                    return Guid.Parse(valueStr);
                }
                return null;
            }
        }

        public string UserName
        {
            get
            {
                if (IsAuthenticated)
                {
                    var valueStr = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
                    return valueStr;
                }
                return null;
            }
        }

        public async Task<ContextUser> GetContextUserAsync()
        {
            ContextUser contextUser = new ContextUser()
            {
                UserId = UserId.GetValueOrDefault(),
                Surname = UserName,
            };
            if (!IsAuthenticated)
            {
                return contextUser;
            }
            var db = _redis.GetDatabase();
            string key = UserId.GetValueOrDefault().ToString();
            string contextUserJson = await db.HashGetAsync(RedisKeys.CONTEXT_USER, key);
            if (string.IsNullOrEmpty(contextUserJson))
            {
                return contextUser;
            }
            return contextUserJson.ToNewtonJsonObject<ContextUser>();
        }

        public async Task SetContextUserAsync(ContextUser contextUser)
        {
            var db = _redis.GetDatabase();
            string key = contextUser.UserId.ToString();
            var contextUserJson = contextUser.ToNewtonJsonString();
            await db.HashSetAsync(RedisKeys.CONTEXT_USER,key, contextUserJson);
        }

        public async Task<List<string>> GetPermissionListAsync()
        {
            if (!IsAuthenticated)
            {
                return new List<string>();
            }

            var contextUser = await GetContextUserAsync();
            return contextUser.Permissions;
        }

        public async Task<bool> IsGrantedAsync(string permissionCode)
        {
            var permissions = await GetPermissionListAsync();
            return permissions.Contains(permissionCode);
        }
    }
}
