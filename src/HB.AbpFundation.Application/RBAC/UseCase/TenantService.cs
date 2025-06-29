using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Frameworks;
using HB.AbpFundation.RBAC.UseCase;
using HB.AbpFundation.Repositories.RBAC;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace HB.AbpFundation.RBAC.Query
{

    public class TenantService : ApplicationService, ITenantService, IScopedDependency
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;

        public TenantService(ITenantRepository tenantRepository, IUserRepository userRepository)
        {
            _tenantRepository = tenantRepository;
            _userRepository = userRepository;
        }
        public async Task<bool> CreateAsync(CreateTenantInput input)
        {
            if (await _tenantRepository.AnyAsync(x => x.Name == input.Name))
            {
                throw new UserFriendlyException("租户名称已存在");
            }
            var tenant = ObjectMapper.Map<CreateTenantInput, Tenant>(input);
            await _tenantRepository.AddAsync(tenant);
            return await _tenantRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _tenantRepository.DeleteAsync(id);
            return await _tenantRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateTenantInput input)
        {
            if (await _tenantRepository.AnyAsync(x => x.Name == input.Name && x.Id != input.Id))
            {
                throw new UserFriendlyException("租户名称已存在");
            }
            var tenant = await _tenantRepository.GetAsync(input.Id, input.ReadOnly);
            tenant = ObjectMapper.Map<UpdateTenantInput, Tenant>(input, tenant);
            await _tenantRepository.UpdateAsync(tenant);
            return await _tenantRepository.SaveChangesAsync();
        }
    }
}
