using System.Collections.Generic;
using System.Threading.Tasks;
using CodeSmithCore;
using HB.AbpFundation;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.Samples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace HB.AbpFundation.Samples;


[Route("api/AbpFundation/[controller]/[action]")]
public class CodeSmithController : AbpFundationController
{
    private readonly ICodeSmithService _sampleAppService;

    public CodeSmithController(ICodeSmithService sampleAppService)
    {
        _sampleAppService = sampleAppService;
    }


    /// <summary>
    /// 生成代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<QueryApiBaseResultDto<GenerateCodeDto>> GenerateCodeAsync([FromBody]GenerateCodeInput input)
    {
        return await HandleAsync(async () =>
        {
           return await _sampleAppService.GenerateCodeAsync(input);
        });
    }
    /// <summary>
    /// 生成VBA脚本
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<QueryApiBaseResultDto<string>> GenerateVbaScriptAsync(string typeName)
    {
        return await HandleAsync( async () => {
            return await _sampleAppService.GenerateVbaScriptAsync(typeName);
        });
        
    }
   

    /// <summary>
    /// 返回所有实体类信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<QueryApiBaseResultDto<List<PersistenceObjectInfo>>> GetAllDomainsAsync()
    {
        return await HandleAsync( async () =>
        {
            return await _sampleAppService.GetAllDomainsAsync();
        });
    }
  
}
