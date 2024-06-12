
using Application.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Identity;
public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IMapper mapper, AppDbContext appDbContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _mapper = mapper;
        _appDbContext = appDbContext;
    }

    public async Task<IResponseWrapper> CreateRoleAsync(CreateRoleRequest request)
    {
        var roleExist = await _roleManager.FindByNameAsync(request.Name);
        if (roleExist is not null)
            return await ResponseWrapper<string>.FailAsync("Role already is exist.");

        var newRole = new AppRole { Name = request.Name, Description = request.Description };

        var identityResult = await _roleManager.CreateAsync(newRole);
        if (identityResult.Succeeded)
            return await ResponseWrapper<string>.SuccessAsync("Role created successfully");
        return await ResponseWrapper<string>.FailAsync(GetIdentityResultErrorDescriptions(identityResult));

    }

    public async Task<IResponseWrapper> DeleteRoleAsync(string roleId)
    {
        var roleInDb = await _roleManager.FindByIdAsync(roleId);
        if (roleInDb is not null)
        {
            if (roleInDb.Name != AppRoles.Admin)
            {
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user,roleInDb.Name))
                    {
                        return await ResponseWrapper.FailAsync($"Role:{roleInDb.Name} is currently assigned to a user");
                    }
                }
                var identityResult= await _roleManager.DeleteAsync(roleInDb);
                if (identityResult.Succeeded)
                    return await ResponseWrapper.SuccessAsync("Role successfully deleted.");
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
            }
            return await ResponseWrapper.FailAsync("Cannot delete Admin Role.");
        }
        return await ResponseWrapper.FailAsync("Role does not exist.");
    }

    public async Task<IResponseWrapper> GetPermissiosAsync(string roleId)
    {
        var roleInDb = await _roleManager.FindByIdAsync(roleId);
        if (roleInDb is not null)
        {
            var allPermissions = AppPermissions.AllPermissions;
            var roleClaimResponse = new RoleClaimResponse
            {
                Role = new()
                {
                    Id = roleId,
                    Name= roleInDb.Name,
                    Description= roleInDb.Description,
                },
                RoleClaims = new()                
            };

            var currentRoleClaims = await GetAllClaimsForRoleAsync(roleId);
            var allPermissionsNames=allPermissions.Select(p=>p.Name).ToList();
            var currentRoleClaimsValues=currentRoleClaims.Select(crc=>crc.ClaimValue).ToList();
            var currentlyAssignedRoleClaimsNames=allPermissionsNames.Intersect(currentRoleClaimsValues).ToList();

            foreach (var permission in allPermissions)
            {
                if (currentlyAssignedRoleClaimsNames.Any(carc=>carc==permission.Name))
                {
                    roleClaimResponse.RoleClaims.Add(new RoleClaimViewModel
                    {
                        RoleId=roleId,
                        ClaimType=AppClaim.Permission,
                        ClaimValue=permission.Name,
                        Description=permission.Description,
                        Group=permission.Group,
                        IsAssignedToRole=true
                    });
                }
                else
                {
                    roleClaimResponse.RoleClaims.Add(new RoleClaimViewModel
                    {
                        RoleId = roleId,
                        ClaimType = AppClaim.Permission,
                        ClaimValue = permission.Name,
                        Description = permission.Description,
                        Group = permission.Group,
                        IsAssignedToRole = false
                    });
                }
            }
            return await ResponseWrapper<RoleClaimResponse>.SuccessAsync(roleClaimResponse);

        }
        return await ResponseWrapper<RoleClaimResponse>.FailAsync("Role does not exist.");
    }

    private async Task<List<RoleClaimViewModel>> GetAllClaimsForRoleAsync(string roleId)
    {
        var roleClaims=await _appDbContext.RoleClaims.Where(rc=>rc.RoleId==roleId).ToListAsync();
        if (roleClaims.Count>0)
        {
            var mappedRoleClaims=_mapper.Map<List<RoleClaimViewModel>>(roleClaims);
            return mappedRoleClaims;
        }
        return new List<RoleClaimViewModel>();
    }

    public async Task<IResponseWrapper> GetRoleByIdAsync(string roleId)
    {
        var roleInDb = await _roleManager.FindByIdAsync(roleId);
        if (roleInDb is not null)
        {
            var mappedRole = _mapper.Map<RoleResponse>(roleInDb);
            return await ResponseWrapper<RoleResponse>.SuccessAsync(mappedRole);
        }
        return await ResponseWrapper.FailAsync("Role does not exist.");
    }

    public async Task<IResponseWrapper> GetRolesAsync()
    {
        var allRoles = await _roleManager.Roles.ToListAsync();
        if (allRoles.Count > 0)
        {
            var mappedRoles = _mapper.Map<List<RoleResponse>>(allRoles);
            return await ResponseWrapper<List<RoleResponse>>.SuccessAsync(mappedRoles);
        }
        return await ResponseWrapper<string>.FailAsync("No roles were found.");

    }

    public async Task<IResponseWrapper> UpdateRoleAsync(UpdateRoleRequest request)
    {
        var roleInDb = await _roleManager.FindByIdAsync(request.RoleId);
        if (roleInDb is not null)
        {
            if (roleInDb.Name != AppRoles.Admin)//Admin role does not updated
            {

                roleInDb.Name = request.RoleName;
                roleInDb.Description = request.RoleDescription;

                var identityResult = await _roleManager.UpdateAsync(roleInDb);
                if (identityResult.Succeeded)
                    return await ResponseWrapper<string>.SuccessAsync("Role updated successfully.");
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDescriptions(identityResult));
            }
            return await ResponseWrapper.FailAsync("Cannot update Admin Role.");
        }
        return await ResponseWrapper.FailAsync("Roles does not exist.");
    }

    private List<string> GetIdentityResultErrorDescriptions(IdentityResult identityResult)
    {
        var errorDescriptions = new List<string>();
        foreach (var error in identityResult.Errors)
        {
            errorDescriptions.Add(error.Description);
        }
        return errorDescriptions;
    }
}
