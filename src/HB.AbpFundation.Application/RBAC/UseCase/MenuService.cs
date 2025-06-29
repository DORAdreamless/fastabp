using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using HB.AbpFundation.Repositories.RBAC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.RBAC.Query
{
    public class MenuService : ApplicationService, IMenuService, IScopedDependency
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<bool> CreateAsync(CreateMenuInput input)
        {
            Menu menu = ObjectMapper.Map<CreateMenuInput, Menu>(input);
            menu.Id = GuidGenerator.Create();
            List<MenuPermission> listMenuPermission = new List<MenuPermission>();
            foreach (var item in input.Permissions)
            {
                MenuPermission menuPermission = new MenuPermission()
                {
                    Id = GuidGenerator.Create(),
                    IsDefault = item.IsDefault,
                    MenuId = menu.Id,
                    PermissionName = item.PermissionName,
                    PermissionCode = item.PermissionCode,
                    SequenceTime = DateTime.Now
                };
                listMenuPermission.Add(menuPermission);
            }
            return await _menuRepository.CreateAsync(menu, listMenuPermission);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _menuRepository.DeleteAsync(id);
            return await _menuRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateMenuInput input)
        {
            Menu menu = await _menuRepository.GetAsync(input.Id, input.ReadOnly);
            menu = ObjectMapper.Map<UpdateMenuInput, Menu>(input, menu);
            List<MenuPermission> listMenuPermission = new List<MenuPermission>();
            foreach (var item in input.Permissions)
            {
                MenuPermission menuPermission = new MenuPermission()
                {
                    Id = GuidGenerator.Create(),
                    IsDefault = item.IsDefault,
                    MenuId = menu.Id,
                    PermissionName = item.PermissionName,
                    PermissionCode = item.PermissionCode,
                    SequenceTime = DateTime.Now
                };
                listMenuPermission.Add(menuPermission);
            }
            return await _menuRepository.UpdateAsync(menu, listMenuPermission);
        }
    }
}
