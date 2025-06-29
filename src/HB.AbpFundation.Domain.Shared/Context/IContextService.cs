using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.AbpFundation.Context
{
    public interface IContextService 
    {
        public bool IsAuthenticated { get; }

        public Guid? UserId { get; }

        public string UserName { get; }

        Task<List<string>> GetPermissionListAsync();

        Task<ContextUser> GetContextUserAsync();

        Task<bool> IsGrantedAsync(string permissionCode);

        Task SetContextUserAsync(ContextUser contextUser);
    }
}
