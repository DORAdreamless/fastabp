using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CodeSmithCore.Helpers;
using CodeSmithCore;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using System.Net.Http.Json;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.Persistences;

namespace HB.AbpFundation.DataSeed
{
    /// <summary>
    /// 持久化对象数据种子
    /// </summary>
    internal class PersistenceObjectDataSeedContributor : IDataSeedContributor, Volo.Abp.DependencyInjection.ITransientDependency
    {
        private readonly AppSettings _appSettings;

        public PersistenceObjectDataSeedContributor(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (_appSettings.Assemblies == null)
            {
                return;
            }
            foreach (var assemblyName in _appSettings.Assemblies)
            {
                var assembly = Assembly.Load(assemblyName);

                var listEntityType = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && typeof(PersistenceObjectBase).IsAssignableFrom(x)).ToList();

                foreach (var entityType in listEntityType)
                {
                    Configs.DataSource.Add(new PersistenceObjectInfo(entityType));
                }
            }

            await Task.Delay(10);
        }
    }

}
