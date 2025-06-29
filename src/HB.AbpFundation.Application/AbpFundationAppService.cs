using HB.AbpFundation.Context;
using HB.AbpFundation.Localization;
using Volo.Abp.Application.Services;

namespace HB.AbpFundation;

public abstract class AbpFundationAppService : ApplicationService
{
    protected IContextService ContextService  => LazyServiceProvider.LazyGetService<IContextService>();

    protected AbpFundationAppService()
    {
        LocalizationResource = typeof(AbpFundationResource);
        ObjectMapperContext = typeof(AbpFundationApplicationModule);

    }
}
