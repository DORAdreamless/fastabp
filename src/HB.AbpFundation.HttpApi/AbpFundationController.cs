using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace HB.AbpFundation;


[Authorize]
public abstract class AbpFundationController : AbpControllerBase
{
    protected AbpFundationController()
    {
        LocalizationResource = typeof(AbpFundationResource);
    }

    protected async Task<QueryApiBaseResultDto<TOut>> HandleAsync<TOut>(Func<Task<TOut>> action)
    {
        QueryApiBaseResultDto<TOut> result = new QueryApiBaseResultDto<TOut>();
        try
        {
            result.Data = await action();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,"111");
            result.Code = -1;
            result.Msg = ex.Message;
        }
        return result;
    }

    protected async Task<QueryApiBaseResultDto<bool>> HandleAsync(Func<Task<bool>> action)
    {
        return await HandleAsync<bool>(action);
    }

   protected QueryApiBaseResultDto<TOut> Handle<TOut>(Func<TOut> action)
    {
        QueryApiBaseResultDto<TOut> result = new QueryApiBaseResultDto<TOut>();
        try
        {
            result.Data =  action();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "111");
            result.Code = -1;
            result.Msg = ex.Message;
        }
        return result;
    }

    protected QueryApiBaseResultDto<bool> Handle(Func<bool> action)
    {
        return Handle<bool>(action);
    }
}
