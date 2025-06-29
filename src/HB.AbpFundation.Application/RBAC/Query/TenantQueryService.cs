using FreeSql;
using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.Context;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using HB.AbpFundation.Repositories.RBAC;
using LinqKit;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

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

    public class UserQueryService : ApplicationService, IUserQueryService
    {
        private readonly IUserRepository _userRepository;
        private readonly IContextService _contextService;

        public UserQueryService(IUserRepository userRepository, IContextService contextService)
        {
            _userRepository = userRepository;
            _contextService = contextService;
        }

        public async Task<UserDto> GetAsync(GetIdInput input)
        {
            var user = await _userRepository.GetAsync(input.Id, input.ReadOnly);
           var roles = await _userRepository.GetUserRolesAsync(input.Id);
            var userDto = ObjectMapper.Map<User, UserDto>(user);
            userDto.RoleIds = roles.Select(x=>x.Id).ToList();
            return userDto;
        }

        public async Task<ContextUser> GetContextUserAsync()
        {
            return await _contextService.GetContextUserAsync();
        }

        public async Task<PagedResultDto<UserDto>> GetListAsync(GetUserInput input)
        {
            var predicate = LinqKit.PredicateBuilder.New<User>(true); // false 表示 OR 条件
            if (!string.IsNullOrWhiteSpace(input.Keywords))
            {
                predicate = predicate.And(x => x.Name.Contains(input.Keywords) || x.UserName.Contains(input.Keywords));
            }
            var list = await _userRepository.GetPagedListAsync(predicate, input.SkipCount, input.MaxResultCount);
            return new PagedResultDto<UserDto>()
            {
                Items = ObjectMapper.Map<IReadOnlyList<User>, List<UserDto>>(list.Items),
                TotalCount = list.TotalCount
            };
        }
    }

    public class TenantQueryService : ApplicationService, ITenantQueryService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantQueryService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<TenantDto> GetAsync(GetIdInput input)
        {
            var tenant = await _tenantRepository.GetAsync(input.Id, input.ReadOnly);
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }



        public async Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantInput input)
        {
            var predicate = LinqKit.PredicateBuilder.New<Tenant>(true); // false 表示 OR 条件
            if (!string.IsNullOrWhiteSpace(input.Keywords))
            {
                predicate = predicate.And(x => x.Name.Contains(input.Keywords) || x.DisplayName.Contains(input.Keywords));
            }
            var list = await _tenantRepository.GetPagedListAsync(predicate, input.SkipCount, input.MaxResultCount, input.Sorting);
            var items = ObjectMapper.Map<IReadOnlyList<Tenant>, List<TenantDto>>(list.Items);
            return new PagedResultDto<TenantDto>()
            {
                Items = items,
                TotalCount = list.TotalCount,
            };
        }
    }
}
