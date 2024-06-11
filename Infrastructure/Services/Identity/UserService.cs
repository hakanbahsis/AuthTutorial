using Application.Services.Identity;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity;
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request)
    {
        var userWithSameEmail=await _userManager.FindByEmailAsync(request.Email);
        if ((userWithSameEmail is not null))
            return await ResponseWrapper.FailAsync("Email already taken.");

        var userWithSameUsername = await _userManager.FindByNameAsync(request.UserName);
        if ((userWithSameUsername is not null))
            return await ResponseWrapper.FailAsync("Username already taken");

        var newUser = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            IsActive = request.ActivateUser,
            EmailConfirmed = request.AutoConfirmEmail
        };

        //Hash and store the hash password
        var password = new PasswordHasher<AppUser>();
        newUser.PasswordHash = password.HashPassword(newUser, request.Password);

        var identityResult=await _userManager.CreateAsync(newUser);
        if (identityResult.Succeeded)
        {
            //Assign to Basic Role
            await _userManager.AddToRoleAsync(newUser,AppRoles.Basic);
            return await ResponseWrapper<string>.SuccessAsync("User registered succesfully.");
        }
        return await ResponseWrapper.FailAsync("User registration failed");

    }
}
