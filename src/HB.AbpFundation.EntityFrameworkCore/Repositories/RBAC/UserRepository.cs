using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Persistences;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Repositories.RBAC
{
    public class UserRepository : Repository<User>, IUserRepository, IScopedDependency
    {
        public UserRepository(FreeSqlDbContext freeSql) : base(freeSql)
        {
        }

        public async Task<bool> CreateAsync(User user, List<UserRole> userRoles)
        {
            await AddAsync(user);
            await Context.Set<UserRole>().AddRangeAsync(userRoles);
            return await SaveChangesAsync();
        }

        public async Task<List<RoleDto>> GetUserRolesAsync(Guid id)
        {
            var items  = await GetAll<UserRole>()
                  .From<Role>()
                  .InnerJoin((ur, r) => ur.RoleId == r.Id)
                  .Where((ur, r) => ur.UserId == id)
                  .ToListAsync((ur, r) => new RoleDto()
                  {
                      Id= r.Id,
                      Name= r.Name,
                      DisplayName= r.DisplayName,
                  });

            return items;
        }

        public async Task<bool> UpdateAsync(User user, List<UserRole> userRoles)
        {
            await UpdateAsync(user);
            await Context.Set<UserRole>().RemoveAsync(x=>x.UserId==user.Id);
            await Context.Set<UserRole>().AddRangeAsync(userRoles);
            return await SaveChangesAsync();
        }
    }
}
