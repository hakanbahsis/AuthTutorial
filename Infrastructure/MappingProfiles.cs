using AutoMapper;
using Common.Responses.Employees;
using Common.Responses.Identity;
using Domain.Entities;
using Infrastructure.Models;

namespace Infrastructure;
public class MappingProfiles:Profile
{
    public MappingProfiles()
    {
        CreateMap<AppUser,UserResponse>().ReverseMap();
        CreateMap<AppRole, RoleResponse>().ReverseMap();
        CreateMap<AppRoleClaim,RoleClaimViewModel>().ReverseMap();

        
    }
}
