using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using HB.AbpFundation.Repositories.RBAC;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace HB.AbpFundation.RBAC.Query
{

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
