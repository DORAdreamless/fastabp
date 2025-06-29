using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using HB.AbpFundation.Repositories.RBAC;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.RBAC.Query
{
    public class RoleService : AbpFundationAppService, IRoleService, IScopedDependency
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> CreateAsync(CreateRoleInput input)
        {
            if (await _roleRepository.AnyAsync(x => x.Name == input.Name))
            {
                throw new UserFriendlyException("角色名称已存在");
            }
            var role = ObjectMapper.Map<CreateRoleInput, Role>(input);
            await _roleRepository.AddAsync(role);
            return await _roleRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _roleRepository.DeleteAsync(id);
            return await _roleRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateRoleInput input)
        {
            if (await _roleRepository.AnyAsync(x => x.Name == input.Name && x.Id != input.Id))
            {
                throw new UserFriendlyException("角色名称已存在");
            }
            var role = await _roleRepository.GetAsync(input.Id);
            role = ObjectMapper.Map<UpdateRoleInput, Role>(input, role);
            await _roleRepository.UpdateAsync(role);
            return await _roleRepository.SaveChangesAsync();
        }
    }
}
