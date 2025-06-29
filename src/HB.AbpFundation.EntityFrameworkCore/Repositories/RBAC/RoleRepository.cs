using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.Persistences;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Repositories.RBAC
{
    public class RoleRepository : Repository<Role>, IRoleRepository, IScopedDependency
    {
        public RoleRepository(FreeSqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}
