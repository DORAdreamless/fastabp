using Volo.Abp.Modularity;

namespace HB.AbpFundation;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class AbpFundationApplicationTestBase<TStartupModule> : AbpFundationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
