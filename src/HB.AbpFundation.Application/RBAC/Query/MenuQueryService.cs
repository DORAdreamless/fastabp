using FreeSql;
using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Repositories.RBAC;
using LinqKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.RBAC.Query
{
    public class MenuQueryService : AbpFundationAppService, IMenuQueryService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPermissionGrantedRepository _permissionGrantedRepository;

        public MenuQueryService(IMenuRepository menuRepository, IPermissionGrantedRepository permissionGrantedRepository)
        {
            _menuRepository = menuRepository;
            _permissionGrantedRepository = permissionGrantedRepository;
        }

        public async Task<ListResultDto<MenuDto>> GetAllAsync(GetMenuInput input)
        {
            var items = await  _menuRepository.GetManyAsync(x => x.Enabled == true);
            var menuPermissions = await _menuRepository.GetAllMenuPermissionsAsync();
            var list= new ListResultDto<MenuDto>()
            {
                Items = ObjectMapper.Map<List<Menu>, List<MenuDto>>(items)
            };

           var permissionGranteds = await _permissionGrantedRepository.GetManyAsync(x => x.ProviderKey == input.ProviderKey && x.ProviderName == input.ProviderName);


            list.Items.ForEach(item =>
            {
                item.Permissions = menuPermissions.Where(x => x.MenuId == item.Id).Select(x => new MenuPermissionInput()
                {
                    IsDefault = x.IsDefault,
                    PermissionCode = x.PermissionCode,
                    PermissionName = x.PermissionName,
                }).OrderBy(x=>x.PermissionCode).ToList();
                item.CheckPermissions = (from p in item.Permissions
                                         join pg in permissionGranteds
                                         on p.PermissionCode equals pg.PermissionCode
                                         select pg.PermissionCode).Distinct().ToList();
                item.CheckPermissionAll = item.CheckPermissions.Count == item.Permissions.Count;

            });
            return list;
        }

        public async Task<MenuDto> GetAsync(GetIdInput input)
        {
           return await _menuRepository.GetDetailAsync(input);
        }
    }
}
