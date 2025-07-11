﻿using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.Persistences;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Repositories.RBAC
{

    public interface ITenantRepository:IRepository<Tenant>
    {

    }
}
