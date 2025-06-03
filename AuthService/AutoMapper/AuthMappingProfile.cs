using AuthService.DomainModel;
using AuthService.Dtos;
using AutoMapper;

namespace AuthService.AutoMapper
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            // Licence mappings
            CreateMap<Licence, LicenceDto>()
                .ForMember(dest => dest.CurrentUserCount, opt => opt.MapFrom(src => src.Tenant.Sum(t => t.Users.Count)))
                .ForMember(dest => dest.AvailableSlots, opt => opt.MapFrom(src => src.NumberOfUser - src.Tenant.Sum(t => t.Users.Count)));

            CreateMap<CreateLicenceDto, Licence>();
            CreateMap<UpdateLicenceDto, Licence>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Tenant mappings
            CreateMap<Tenant, TenantDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count))
                .ForMember(dest => dest.LicenceName, opt => opt.Ignore()); // Will be set in service if needed

            CreateMap<CreateTenantDto, Tenant>();
            CreateMap<UpdateTenantDto, Tenant>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // User mappings
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant.Name))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
                .ForMember(dest => dest.Permissions, opt => opt.Ignore()); // Will be calculated in service

            CreateMap<CreateUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.Pending));

            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Role mappings
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.UserRoles.Count));

            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore());

            CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Permission mappings
            CreateMap<Permission, PermissionDto>();
            CreateMap<CreatePermissionDto, Permission>();
            CreateMap<UpdatePermissionDto, Permission>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
