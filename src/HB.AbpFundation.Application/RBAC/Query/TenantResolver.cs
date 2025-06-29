using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.RBAC.Query
{
    public class TenantResolver: ApplicationService,ISingletonDependency
    {
        private readonly IDistributedCache<Tenant> _tenantCache;

        public TenantResolver(IDistributedCache<Tenant> tenantCache)
        {
            this._tenantCache = tenantCache;
        }

        public async Task<Tenant> GetContextTenantAsync(string name)
        {
            return new Tenant()
            {
                Name = name,
                MasterConnectionString = "Server=192.168.124.9;Port=3306;Database=aicollege;Uid=root;Pwd=chenxy888;SslMode=none;",
                ReadOnlyConnectionStrings = "[\"Server=192.168.124.9;Port=3306;Database=aicollege;Uid=root;Pwd=chenxy888;SslMode=none;\"]"
            };
            return await  _tenantCache.GetAsync(name);
           
        }
    }
}
