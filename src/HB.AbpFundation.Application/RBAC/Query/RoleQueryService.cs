using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Repositories.RBAC;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.RBAC.Query
{
    public class RoleQueryService : AbpFundationAppService, IRoleQueryService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleQueryService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ListResultDto<RoleDto>> GetAllAsync()
        {
            var items = await _roleRepository.GetManyAsync(x => x.Enabled);
            return new ListResultDto<RoleDto>()
            {
                Items = ObjectMapper.Map<List<Role>, List<RoleDto>>(items)
            };
        }

        public async Task<PagedResultDto<RoleDto>> GetListAsync(GetRoleInput input)
        {
            var predicate = LinqKit.PredicateBuilder.New<Role>(true); // false 表示 OR 条件
            if (!string.IsNullOrWhiteSpace(input.Keywords))
            {
                predicate = predicate.And(x => x.Name.Contains(input.Keywords) || x.DisplayName.Contains(input.Keywords));
            }
            var list = await _roleRepository.GetPagedListAsync(predicate, input.SkipCount, input.MaxResultCount);
            return new PagedResultDto<RoleDto>()
            {
                Items = ObjectMapper.Map<IReadOnlyList<Role>, List<RoleDto>>(list.Items),
                TotalCount = list.TotalCount
            };
        }

        public async Task<RoleDto> GetAsync(GetIdInput input)
        {
            var role = await _roleRepository.GetAsync(input.Id, input.ReadOnly) ;
            return ObjectMapper.Map<Role,RoleDto>(role) ;
        }
    }
}
