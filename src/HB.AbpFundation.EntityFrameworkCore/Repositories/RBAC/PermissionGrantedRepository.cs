using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.Persistences;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Repositories.RBAC
{
    public class PermissionGrantedRepository : Repository<PermissionGranted>, IPermissionGrantedRepository, IScopedDependency
    {
        public PermissionGrantedRepository(FreeSqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}
