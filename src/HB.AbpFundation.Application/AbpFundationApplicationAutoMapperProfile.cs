using AutoMapper;
using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;

namespace HB.AbpFundation;

public class AbpFundationApplicationAutoMapperProfile : Profile
{
    public AbpFundationApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<CreateTenantInput, Tenant>(MemberList.None);
        CreateMap<UpdateTenantInput, Tenant>(MemberList.None);
        CreateMap<Tenant, TenantDto>(MemberList.None);

        CreateMap<CreateUserInput, User>(MemberList.None);
        CreateMap<UpdateUserInput, User>(MemberList.None);
        CreateMap<User,UserDto>(MemberList.None);

        CreateMap<CreateMenuInput, Menu>(MemberList.None);
        CreateMap<UpdateMenuInput, Menu>(MemberList.None);
        CreateMap<Menu, MenuDto>(MemberList.None);

        CreateMap<CreateRoleInput, Role>(MemberList.None);
        CreateMap<UpdateRoleInput, Role>(MemberList.None);
        CreateMap<Role, RoleDto>(MemberList.None);
    }
}
