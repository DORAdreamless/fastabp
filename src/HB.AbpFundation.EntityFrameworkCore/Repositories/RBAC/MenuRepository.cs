using FreeSql;
using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Repositories.RBAC
{
    public class MenuRepository : Repository<Menu>, IMenuRepository, IScopedDependency
    {
        public MenuRepository(FreeSqlDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CreateAsync(Menu menu, List<MenuPermission> listMenuPermission)
        {
            await AddAsync(menu);
            await Context.Set<MenuPermission>().AddRangeAsync(listMenuPermission);
            return await SaveChangesAsync();
        }

        public async Task<MenuDto> GetDetailAsync(GetIdInput input)
        {
            var menu = await GetAsync(input.Id,input.ReadOnly);
            var permissions= await  GetAll<MenuPermission>().Where(x => x.MenuId == input.Id).ToListAsync();
            return new MenuDto
            {
                Id = menu.Id,
                Component = menu.Component,
                CreationTime = menu.CreationTime,
                CreatorId = menu.CreatorId,
                CreatorName = menu.CreatorName,
                DisplayName = menu.DisplayName,
                Icon = menu.Icon,
                LastModificationTime = menu.LastModificationTime,
                LastModifierId = menu.LastModifierId,
                LastModifierName = menu.LastModifierName,
                ParentId = menu.ParentId,
                Remarks = menu.Remarks,
                Url = menu.Url,
                SequenceTime = menu.SequenceTime,
                Enabled = menu.Enabled,
                Permissions = permissions.Select(x => new MenuPermissionInput()
                {
                    IsDefault = x.IsDefault,
                    PermissionCode = x.PermissionCode,
                    PermissionName = x.PermissionName,
                }).OrderBy(x=>x.PermissionCode).ToList()
            };
        }

        public async Task<bool> UpdateAsync(Menu menu, List<MenuPermission> listMenuPermission)
        {
            await UpdateAsync(menu);
            await Context.Set<MenuPermission>().RemoveAsync(x=>x.MenuId==menu.Id);
            await Context.Set<MenuPermission>().AddRangeAsync(listMenuPermission);
            return await SaveChangesAsync();
        }

        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
            await Context.Set<MenuPermission>().RemoveAsync(x => x.MenuId == id);
        }

        public async Task<List<MenuPermission>> GetAllMenuPermissionsAsync()
        {
           return await GetAll<MenuPermission>().ToListAsync();
        }
    }
}
