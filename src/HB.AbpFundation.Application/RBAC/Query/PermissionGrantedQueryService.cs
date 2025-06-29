using FreeSql;
using HB.AbpFundation.Repositories.RBAC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.RBAC.Query
{
    public class PermissionGrantedQueryService : AbpFundationAppService, IPermissionGrantedQueryService
    {
        private readonly IPermissionGrantedRepository _permissionGrantedRepository;

        public PermissionGrantedQueryService(IPermissionGrantedRepository permissionGrantedRepository)
        {
            _permissionGrantedRepository = permissionGrantedRepository;
        }

        public async Task<ListResultDto<string>> GetUserGrantedAsync(string userId,List<string> roleIds)
        {
          var  permissionGranteds = await  _permissionGrantedRepository.GetManyAsync(x => (x.ProviderKey == userId && x.ProviderName == "U" )|| (x.ProviderName == "R" && roleIds.Contains(x.ProviderKey)));
            return new ListResultDto<string>()
            {
                Items = permissionGranteds.Select(x => x.PermissionCode).Distinct().ToList()
            };
        }
    }
}
