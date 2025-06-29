using System;
using System.Threading.Tasks;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.RBAC
{
    [Route("api/AbpFundation/[controller]/[action]")]
    public class PermissionGrantedController : AbpFundationController
    {
        private readonly IPermissionGrantedService _permissionGrantedService;

        public PermissionGrantedController(IPermissionGrantedService permissionGrantedService)
        {
            _permissionGrantedService = permissionGrantedService;
        }

        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> GrantedAsync([FromBody] GrantedInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _permissionGrantedService.GrantedAsync(input);
            });
        }
    }
    /// <summary>
    /// 菜单接口
    /// </summary>
    [Route("api/AbpFundation/[controller]/[action]")]
    public class MenuController : AbpFundationController
    {
        private readonly IMenuService _menuService;
        private readonly IMenuQueryService _menuQueryService;

        public MenuController(IMenuService menuService, IMenuQueryService menuQueryService)
        {
            _menuService = menuService;
            _menuQueryService = menuQueryService;
        }
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> CreateAsync([FromBody] CreateMenuInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _menuService.CreateAsync(input);
            });
        }
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> UpdateAsync([FromBody] UpdateMenuInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _menuService.UpdateAsync(input);
            });
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<QueryApiBaseResultDto<bool>> DeleteAsync(Guid id)
        {
            return await HandleAsync(async () =>
            {
                return await _menuService.DeleteAsync(id);
            });
        }
        [HttpGet]
        public async Task<QueryApiBaseResultDto<MenuDto>> GetAsync([FromQuery] GetIdInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _menuQueryService.GetAsync(input);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<QueryApiBaseResultDto<ListResultDto<MenuDto>>> GetAllAsync([FromQuery] GetMenuInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _menuQueryService.GetAllAsync(input);
            });
        }
    }
}
