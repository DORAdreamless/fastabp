using FreeSql;
using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.Context;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Repositories.RBAC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HB.AbpFundation.RBAC.Query
{
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
}
