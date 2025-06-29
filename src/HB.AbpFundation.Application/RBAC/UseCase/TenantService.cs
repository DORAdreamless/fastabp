using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.Context;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Frameworks;
using HB.AbpFundation.Helpers;
using HB.AbpFundation.RBAC.UseCase;
using HB.AbpFundation.Repositories.RBAC;
using HB.AbpFundation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

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

    public class UserService : AbpFundationAppService, IUserService, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IContextService _contextUservice;
        private readonly IPermissionGrantedQueryService _permissionGrantedQueryService;
        private readonly INumberService _numberService;



        public UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IContextService contextUservice, IPermissionGrantedQueryService permissionGrantedQueryService, INumberService numberService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _contextUservice = contextUservice;
            _permissionGrantedQueryService = permissionGrantedQueryService;
            _numberService = numberService;
        }

        public async Task<bool> CreateAsync(CreateUserInput input)
        {
            var user = ObjectMapper.Map<CreateUserInput, User>(input);
            if (await _userRepository.AnyAsync(x => x.PhoneNumber == input.PhoneNumber))
            {
                throw new UserFriendlyException("手机号码已存在");
            }
            if (!string.IsNullOrEmpty(input.Email))
            {
                if (await _userRepository.AnyAsync(x => x.NormalizedEmail == input.Email.ToUpper()))
                {
                    throw new UserFriendlyException("邮箱已存在");
                }
                user.SetEmail(input.Email);
            }
            var userNames = await _numberService.GetUserNameListAsync();
            user.SetPassword("123456");
            user.ShouldChangePasswordOnNextLogin = true;
            user.Id = GuidGenerator.Create();
            user.SetUserName(userNames[0]);
            List<UserRole> userRoles = new List<UserRole>();
            foreach (var roleId in input.RoleIds)
            {
                UserRole userRole = new UserRole()
                {
                    RoleId = roleId,
                    UserId = user.Id
                };
                userRoles.Add(userRole);
            }
            return await _userRepository.CreateAsync(user, userRoles);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<JWTToken> LoginAsync(LoginInput input)
        {
            var user = await _userRepository.GetAsync(x => x.NormalizedUserName == input.UserName || x.NormalizedEmail == input.UserName || x.PhoneNumber == input.UserName);
            if (user == null)
            {
                throw new UserFriendlyException("账号或密码错误");
            }
            if (user.PasswordHash != PasswordHelper.GetPasswordHash(input.Password, user.SecurityStamp))
            {
                user.AccessFailedCount++;
                if (user.AccessFailedCount > 5)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.Now.AddMinutes(15);
                }
                await _userRepository.UpdateAsync(user);
                await _userRepository.SaveChangesAsync();
                throw new UserFriendlyException("账号或密码错误");
            }
            user.AccessFailedCount = 0;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

          List<RoleDto> roles =await  _userRepository.GetUserRolesAsync(user.Id);
            var roleIds = roles.Select(x=>x.Id.ToString()).ToList();
           var permissionGranteds= await _permissionGrantedQueryService.GetUserGrantedAsync(user.Id.ToString(), roleIds);
            await _contextUservice.SetContextUserAsync(new ContextUser()
            {
                Avatar = user.Avatar,
                DepartmentId = Guid.Empty,
                DepartmentName = "",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                UserId = user.Id,
                UserName = user.UserName,
                RoleNames = roles.Select(x=>x.Name).ToList(),
                Permissions = permissionGranteds.Items.ToList(),
                RoleDescriptions = string.Join(",", roles.Select(x=>x.DisplayName)),
                Introduction = user.Introduction,
                Name = user.Name
                 
            });

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
            };
            return await _jwtTokenService.GenerateTokenAsync(user.Id, claims);
        }

        public async Task<bool> UpdateAsync(UpdateUserInput input)
        {
            var user = await _userRepository.GetAsync(input.Id);

            if (await _userRepository.AnyAsync(x => x.PhoneNumber == input.PhoneNumber && x.Id != input.Id))
            {
                throw new UserFriendlyException("手机号码已存在");
            }
            if (!string.IsNullOrEmpty(input.Email))
            {
                if (await _userRepository.AnyAsync(x => x.NormalizedEmail == input.Email.ToUpper() && x.Id != input.Id))
                {
                    throw new UserFriendlyException("邮箱已存在");
                }
                user.SetEmail(input.Email);
            }

            user = ObjectMapper.Map<UpdateUserInput, User>(input, user);
            List<UserRole> userRoles = new List<UserRole>();
            foreach (var roleId in input.RoleIds)
            {
                UserRole userRole = new UserRole()
                {
                    RoleId = roleId,
                    UserId = user.Id
                };
                userRoles.Add(userRole);
            }
            return await _userRepository.UpdateAsync(user, userRoles);
        }

        public async Task<bool> UpdatePasswordAsync(UpdatePasswordInput input)
        {
            var user = await _userRepository.GetAsync(x => x.Id == ContextService.UserId);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            if (user.PasswordHash != PasswordHelper.GetPasswordHash(input.CurrentPassword, user.SecurityStamp))
            {
                throw new UserFriendlyException("当前密码错误,请确认");
            }
            user.SetPassword(input.NewPassword);
            await _userRepository.UpdateAsync(user);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateProfileAsync(UpdateUserProfileInput input)
        {
            var user = await _userRepository.GetAsync(x => x.Id == ContextService.UserId);
            user.Introduction = input.Introduction;
            user.Surname = input.Surname;
            await _userRepository.UpdateAsync(user);
            return await _userRepository.SaveChangesAsync();
        }
    }
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
