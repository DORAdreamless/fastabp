using Volo.Abp.Modularity;

namespace HB.AbpFundation;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class AbpFundationDomainTestBase<TStartupModule> : AbpFundationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
