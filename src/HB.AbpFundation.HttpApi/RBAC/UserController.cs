using HB.AbpFundation.Context;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.RBAC
{

    /// <summary>
    /// 菜单接口
    /// </summary>
    [Route("api/AbpFundation/[controller]/[action]")]
    public class UserController : AbpFundationController
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IUserService _userService;

        public UserController(IUserQueryService userQueryService, IUserService userService)
        {
            _userQueryService = userQueryService;
            _userService = userService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<QueryApiBaseResultDto<JWTToken>> LoginAsync([FromBody] LoginInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _userService.LoginAsync(input);
            });
        }
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> CreateAsync([FromBody] CreateUserInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _userService.CreateAsync(input);
            });
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> UpdateAsync([FromBody] UpdateUserInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _userService.UpdateAsync(input);
            });
        }
        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> UpdateProfileAsync([FromBody] UpdateUserProfileInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _userService.UpdateProfileAsync(input);
            });
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> UpdatePasswordAsync([FromBody] UpdatePasswordInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _userService.UpdatePasswordAsync(input);
            });
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<QueryApiBaseResultDto<bool>> DeleteAsync(Guid id)
        {
            return await HandleAsync(async () =>
            {
                return await _userService.DeleteAsync(id);
            });
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<QueryApiBaseResultDto<UserDto>> GetAsync([FromQuery] GetIdInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _userQueryService.GetAsync(input);
            });
        }

        [HttpGet]
        public async Task<QueryApiBaseResultDto<PagedResultDto<UserDto>>> GetListAsync([FromQuery] GetUserInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _userQueryService.GetListAsync(input);
            });
        }

        [HttpGet]
        public async Task<QueryApiBaseResultDto<ContextUser>> GetContextUserAsync()
        {
            return await HandleAsync(async () =>
            {
                return await _userQueryService.GetContextUserAsync();
            });
        }
    }
}
