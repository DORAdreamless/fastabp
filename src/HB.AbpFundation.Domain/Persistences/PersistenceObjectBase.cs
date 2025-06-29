using FreeSql;
using HB.AbpFundation.Context;
using NPOI.Util;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace HB.AbpFundation.Persistences
{
    public abstract class PersistenceObjectBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [StringLength(20)]
        public string CreatorName { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public Guid? LastModifierId { get; set; }
        /// <summary>
        /// 最后修改人姓名
        /// </summary>
        [StringLength(20)]
        public string LastModifierName { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public Guid? DeleterId { get; set; }
        /// <summary>
        /// 删除人姓名
        /// </summary>
        [StringLength(20)]
        public string DeleterName { get; set; }


    }

    public interface IRepository<TEntity> where TEntity : PersistenceObjectBase
    {
        Task<bool> AnyAsync(bool readOnly = true, bool includeDeleted = false);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false);

        Task<long> CountAsync(bool readOnly = true, bool includeDeleted = false);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false);

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task UpdateAsync(TEntity entity);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(Guid id);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities);

        Task DeleteRangeAsync(IEnumerable<Guid> ids);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task HardDeleteAsync(TEntity entity);

        Task HardDeleteAsync(Guid id);

        Task HardDeleteRangeAsync(IEnumerable<TEntity> entities);

        Task HardDeleteRangeAsync(IEnumerable<Guid> ids);

        Task HardDeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync(Guid id, bool readOnly = true, bool includeDeleted = false);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false);

        Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false);

        Task<PagedResultDto<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting = "CreationTime desc", bool readOnly = true, bool includeDeleted = false);

        Task<PagedResultDto<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate, int skipCount, int maxResultCount, string sorting = "CreationTime desc", bool readOnly = true, bool includeDeleted = false);

        Task<bool> SaveChangesAsync();
    }

    public class FreeSqlDbContext : IScopedDependency
    {
        private readonly IFreeSql fsql;
        private readonly IContextService contextService;

        public FreeSqlDbContext(IFreeSql fsql, IContextService contextService)
        {
            this.fsql = fsql;
            _dbContext = fsql.CreateDbContext();
            this.contextService = contextService;
        }

        public IFreeSql GetOrm()
        {
            return fsql;
        }

        DbContext _dbContext;
        public DbContext GetContext()
        {
            return _dbContext;
        }

        public TEntity CreateAuditing<TEntity>(TEntity entity) where TEntity : PersistenceObjectBase
        {
            if (entity.CreationTime == DateTime.MinValue)
            {
                entity.CreationTime = DateTime.Now;
            }
            if (contextService.IsAuthenticated)
            {
                entity.CreatorId = contextService.UserId;
                entity.CreatorName = contextService.UserName;
            }
            return entity;
        }

        public TEntity ModifyAuditing<TEntity>(TEntity entity) where TEntity : PersistenceObjectBase
        {
            if (entity.LastModificationTime == null)
            {
                entity.LastModificationTime = DateTime.Now;
            }
            if (contextService.IsAuthenticated)
            {
                entity.LastModifierId = contextService.UserId;
                entity.LastModifierName = contextService.UserName;
            }
            return entity;
        }

        public TEntity DeleteAuditing<TEntity>(TEntity entity) where TEntity : PersistenceObjectBase
        {
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            if (contextService.IsAuthenticated)
            {
                entity.DeleterId = contextService.UserId;
                entity.DeleterName = contextService.UserName;
            }
            return entity;
        }
    }

    public class GlobalFilters
    {
        public const string SoftDelete = "SoftDelete";
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : PersistenceObjectBase
    {
        public Repository(FreeSqlDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.FreeSql = dbContext.GetOrm();
            this.Context = dbContext.GetContext();
            this.Set = this.Context.Set<TEntity>();
        }

        protected FreeSqlDbContext DbContext { get; set; }

        protected IFreeSql FreeSql { get; set; }

        protected DbContext Context { get; set; }

        protected DbSet<TEntity> Set { get; set; }

        protected ISelect<TEntity> GetAll(bool readOnly = true, bool includeDeleted = false)
        {

            var query = Context.Set<TEntity>().Select;
            if (!readOnly)
            {
                query = query.Master();
            }
            if (includeDeleted)
            {
                query = query.DisableGlobalFilter(GlobalFilters.SoftDelete);
            }
            return query;
        }

        protected ISelect<T> GetAll<T>(bool readOnly = true, bool includeDeleted = false) where T : PersistenceObjectBase
        {
            var query = Context.Set<T>().Select;
            if (!readOnly)
            {
                query = query.Master();
            }
            if (includeDeleted)
            {
                query = query.DisableGlobalFilter(GlobalFilters.SoftDelete);
            }
            return query;
        }

        public virtual async Task  AddAsync(TEntity entity)
        {
            entity = DbContext.CreateAuditing(entity);
            await Set.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                 DbContext.CreateAuditing(entity);
            }
            await Set.AddRangeAsync(entities);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            DbContext.DeleteAuditing(entity);
            await Set.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetAll().Where(x => x.Id == id).FirstAsync();
            if (entity != null)
            {
                DbContext.DeleteAuditing(entity);
                await Set.UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetAll().Where(predicate).ToListAsync();
            foreach (var entity in entities)
            {
                DbContext.DeleteAuditing(entity);
            }
            await Set.UpdateRangeAsync(entities);
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DbContext.DeleteAuditing(entity);
            }
            await Set.UpdateRangeAsync(entities);
        }

        public async Task DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var entities = await GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                DbContext.DeleteAuditing(entity);
            }
            await Set.UpdateRangeAsync(entities);
        }



        public  async Task<TEntity> GetAsync(Guid id, bool readOnly = true, bool includeDeleted = false)
        {
            return await GetAll(readOnly, includeDeleted).Where(x => x.Id == id).FirstAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false)
        {
            return await GetAll(readOnly, includeDeleted).Where(predicate).FirstAsync();
        }

        public async Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false)
        {
            return await GetAll(readOnly, includeDeleted).Where(predicate).ToListAsync();
        }

        public Task HardDeleteAsync(TEntity entity)
        {
            Set.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task HardDeleteAsync(Guid id)
        {
            await Set.RemoveAsync(x => x.Id == id);
        }

        public async Task HardDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await Set.RemoveAsync(predicate);
        }

        public Task HardDeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task HardDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            await Set.RemoveAsync(x => ids.Contains(x.Id));
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbContext.ModifyAuditing(entity);
            await Set.UpdateAsync(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DbContext.ModifyAuditing(entity);
            }
            await Set.UpdateRangeAsync(entities);
        }

        public async Task<PagedResultDto<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting = "CreationTime desc", bool readOnly = true, bool includeDeleted = false)
        {
            long totalCount = await GetAll(readOnly, includeDeleted).CountAsync();
            var items = await GetAll(readOnly, includeDeleted).OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
            return new PagedResultDto<TEntity>()
            {
                Items = items,
                TotalCount = totalCount,
            };
        }

        public async Task<PagedResultDto<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate, int skipCount, int maxResultCount, string sorting = "CreationTime desc", bool readOnly = true, bool includeDeleted = false)
        {
            long totalCount = await GetAll(readOnly, includeDeleted).Where(predicate).CountAsync();
            var items = await GetAll(readOnly, includeDeleted).Where(predicate).OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
            return new PagedResultDto<TEntity>()
            {
                Items = items,
                TotalCount = totalCount,
            };
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        

        public async Task<bool> AnyAsync(bool readOnly = true, bool includeDeleted = false)
        {
           return await GetAll(readOnly, includeDeleted).AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false)
        {
            return await GetAll(readOnly, includeDeleted).Where(predicate).AnyAsync();
        }

        public async Task<long> CountAsync(bool readOnly = true, bool includeDeleted = false)
        {
            return await GetAll(readOnly, includeDeleted).CountAsync();
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, bool readOnly = true, bool includeDeleted = false)
        {
            return await GetAll(readOnly, includeDeleted).Where(predicate).CountAsync();
        }
    }
}
