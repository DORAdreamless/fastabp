﻿using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.Persistences;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Repositories.RBAC
{

    public class TenantRepository : Repository<Tenant>, ITenantRepository, IScopedDependency
    {
        public TenantRepository(FreeSqlDbContext freeSql) : base(freeSql)
        {
        }
    }
}
