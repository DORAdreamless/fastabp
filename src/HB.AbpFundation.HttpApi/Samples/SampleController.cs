using System;
using System.Threading.Tasks;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.Samples;

[Area(AbpFundationRemoteServiceConsts.ModuleName)]
[RemoteService(Name = AbpFundationRemoteServiceConsts.RemoteServiceName)]
[Route("api/AbpFundation/sample")]
public class SampleController : AbpFundationController, ISampleAppService
{
    private readonly ISampleAppService _sampleAppService;
    private readonly ITenantQueryService _tenantQueryService;
    private readonly ITenantService _tenantService;
    private readonly IUserQueryService _userQueryService;

    public SampleController(ISampleAppService sampleAppService, ITenantQueryService tenantQueryService, ITenantService tenantService, IUserQueryService userQueryService)
    {
        _sampleAppService = sampleAppService;
        _tenantQueryService = tenantQueryService;
        _tenantService = tenantService;
        _userQueryService = userQueryService;
    }

    [HttpGet]
    public async Task<QueryApiBaseResultDto<UserDto>> GetAsync(Guid id,string tid=null,bool readOnly = true)
    {
        return await HandleAsync(async () =>
        {
            return await _userQueryService.GetAsync(new GetIdInput()
            {
                Id = id,
                ReadOnly = readOnly
            });
        });
    }

  

    [HttpGet]
    [Route("list")]
    public async Task<QueryApiBaseResultDto<PagedResultDto<TenantDto>>> GetListAsync(GetTenantInput input)
    {
        return await HandleAsync(async () =>
        {
            return await _tenantQueryService.GetListAsync(input);
        });
    }
    [HttpPost]
    public async Task<QueryApiBaseResultDto<bool>> CreateAsync(CreateTenantInput input)
    {
        return await HandleAsync(async () =>
        {
            return await _tenantService.CreateAsync(input);
        });

    }
}
