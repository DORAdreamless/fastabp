using System.Collections.Generic;
using System.Threading.Tasks;
using CodeSmithCore;
using Volo.Abp.Application.Services;

namespace HB.AbpFundation.Samples;

public interface ICodeSmithService : IApplicationService
{
    /// <summary>
    /// 获取所有实体
    /// </summary>
    /// <returns></returns>
    Task<List<PersistenceObjectInfo>> GetAllDomainsAsync();

    /// <summary>
    /// 生成代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<GenerateCodeDto> GenerateCodeAsync(GenerateCodeInput input);
    /// <summary>
    /// 生成VBA脚本
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    Task<string> GenerateVbaScriptAsync(string typeName);

  
}
public class GenerateCodeInput
{
    public string CompanyName { get; set; } = "HB";

    public string ModuleName { get; set; } = "Fundation";

    public string FunctionName { get; set; } = "RBAC";


    public List<string> TypeNames { get; set; } = new List<string>();

    /// <summary>
    /// 权限码
    /// </summary>
    public string PermissionCode { get; set; }
}

public class GenerateCodeDto
{
    public string ModelCreatingExtensions { get; set; }
    public string DbContext { get; set; }
    public string AutoMapperProfile { get; set; }
}