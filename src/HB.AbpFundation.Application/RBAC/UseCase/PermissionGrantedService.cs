using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using HB.AbpFundation.Repositories.RBAC;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.RBAC.Query
{
    public class PermissionGrantedService : AbpFundationAppService, IPermissionGrantedService, IScopedDependency
    {
        private readonly IPermissionGrantedRepository _permissionGrantedRepository;

        public PermissionGrantedService(IPermissionGrantedRepository permissionGrantedRepository)
        {
            _permissionGrantedRepository = permissionGrantedRepository;
        }

        public async Task<bool> GrantedAsync(GrantedInput input)
        {
            await _permissionGrantedRepository.HardDeleteAsync(x => x.ProviderKey == input.ProviderKey && x.ProviderName == input.ProviderName);
            foreach (var permissionCode in input.PermissionCodes)
            {
                await _permissionGrantedRepository.AddAsync(new PermissionGranted()
                {
                    ProviderKey = input.ProviderKey,
                    ProviderName = input.ProviderName,
                    PermissionCode = permissionCode
                });
            }
            return await _permissionGrantedRepository.SaveChangesAsync();
        }
    }
}
