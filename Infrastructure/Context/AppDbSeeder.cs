using Common.Authorization;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;
public class AppDbSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppDbContext _appDbContext;

    public AppDbSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppDbContext appDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _appDbContext = appDbContext;
    }

    public async Task SeedDatabaseAsync()
    {
        //check for pending and apply any if
        await CheckAndApplyPendingMigrationAsync();
        //seed roles
        await SeedRolesAsync();
        //seed user(basic)
        await SeedBasicUserAsync();
        //seed user(admin)
        await SeedAdminUserAsync();
    }

    private async Task CheckAndApplyPendingMigrationAsync()
    {
        if (_appDbContext.Database.GetPendingMigrations().Any())
        {
            await _appDbContext.Database.MigrateAsync();
        }
    }

    private async Task SeedBasicUserAsync()
    {
        var basicUser = new AppUser
        {
            FirstName="John",
            LastName="Doe",
            Email="johnd@abc.com",
            NormalizedEmail="JOHND@ABC.COM",
            UserName="johnd",
            NormalizedUserName="JOHND",
            EmailConfirmed=true,
            PhoneNumberConfirmed=true,
            IsActive=true
        };
        if (!await _userManager.Users.AnyAsync(u=>u.Email=="johnd@abc.com"))
        {
            var password = new PasswordHasher<AppUser>();
            basicUser.PasswordHash = password.HashPassword(basicUser, AppCredentials.Password);
            await _userManager.CreateAsync(basicUser);
        }
        //Assign role to user
        if (!await _userManager.IsInRoleAsync(basicUser,AppRoles.Basic))
        {
            await _userManager.AddToRoleAsync(basicUser, AppRoles.Basic);
        }
    }
    private async Task SeedAdminUserAsync()
    {
        string adminUserName = AppCredentials.Email[..AppCredentials.Email.IndexOf('@')].ToLowerInvariant();
        var adminUser = new AppUser
        {
            FirstName = "Hakan",
            LastName = "Bahşiş",
            Email = AppCredentials.Email,
            UserName = adminUserName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            NormalizedEmail = AppCredentials.Email.ToUpperInvariant(),
            NormalizedUserName = adminUserName.ToUpperInvariant(),
            IsActive = true
        };

        if (!await _userManager.Users.AnyAsync(u => u.Email == AppCredentials.Email))
        {
            var password = new PasswordHasher<AppUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, AppCredentials.Password);
            await _userManager.CreateAsync(adminUser);
        }

        // Assign role to user
        if (!await _userManager.IsInRoleAsync(adminUser, AppRoles.Basic)
            && !await _userManager.IsInRoleAsync(adminUser, AppRoles.Admin))
        {
            await _userManager.AddToRolesAsync(adminUser, AppRoles.DefaultRoles);
        }
    }

    private async Task SeedRolesAsync()
    {
        foreach (var roleName in AppRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.FirstOrDefaultAsync(r=>r.Name==roleName)
                is not AppRole role )
            {
                role = new AppRole
                {
                    Name = roleName,
                    Description = $"{roleName} Role."
                };
                await _roleManager.CreateAsync(role);
            }

            //Assign Permissions
            if (roleName==AppRoles.Admin)
            {
                //Admin
                await AssignPermissionsToRoleAsync(role,AppPermissions.AdminPermissions);
            }
            else if(roleName==AppRoles.Basic)
            {
                //Basic
                await AssignPermissionsToRoleAsync(role,AppPermissions.BasicPermissions);
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(AppRole role,IReadOnlyList<AppPermission> permissions)
    {
        var currentClaims=await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!currentClaims.Any(claim=>claim.Type==AppClaim.Permission && claim.Value==permission.Name))
            {
                await _appDbContext.RoleClaims.AddAsync(new AppRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.Name,
                    Description=permission.Description,
                    Group=permission.Group
                });
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
