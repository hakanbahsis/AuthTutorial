using Application.Services.Identity;
using AutoMapper;
using Common.Authorization;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Common.Responses.Wrappers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Identity;
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IResponseWrapper> ChangeUserPasswordAsync(ChangePasswordRequest request)
    {
        var userInDb = await _userManager.FindByIdAsync(request.UserId);
        if (userInDb is not null)
        {
            var identityResult = await _userManager.ChangePasswordAsync(userInDb, request.CurrentPassword, request.NewPassword);
            if(identityResult.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("User password updated.");
            }
            return await ResponseWrapper.FailAsync("Failed to update user password.");
        }
        return await ResponseWrapper.FailAsync("User does not exist.");
    }

    public async Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request)
    {
        var userInDb = await _userManager.FindByIdAsync(request.UserId);
        if(userInDb is not null)
        {
            userInDb.IsActive = request.Activate;
            var identityResult=await _userManager.UpdateAsync(userInDb);
            if(identityResult.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync(request.Activate ? "User actived succesfully." : "User de-activated succesfully.");
            }
            return await ResponseWrapper.FailAsync(request.Activate ? "Failed to activate user." : "Failed to de-activate user.");
        }
        return await ResponseWrapper.FailAsync("User does not exist.");
    }

    public async Task<IResponseWrapper> GetAllUsersAsync()
    {
        var usersInDb=await _userManager.Users.ToListAsync();
        if (usersInDb.Count>0)
        {
            var mappedUsers=_mapper.Map<List<UserResponse>>(usersInDb);
            return await ResponseWrapper<List<UserResponse>>.SuccessAsync(mappedUsers);
        }
        return await ResponseWrapper.FailAsync("No users were found.");
    }

    public async Task<IResponseWrapper> GetUserByIdAsync(Guid userId)
    {
        var userInDb = await _userManager.FindByIdAsync(userId.ToString());
        if (userInDb is not null)
        {
            //Mapper
            var mappedUser=_mapper.Map<UserResponse>(userInDb);
            return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
        }
        return await ResponseWrapper.FailAsync("User does not exist.");
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

    public async Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request)
    {
        var userInDb=await _userManager.FindByIdAsync(request.UserId.ToString());
        if (userInDb is not null)
        {
            //Update
            userInDb.FirstName= request.FirstName;
            userInDb.LastName= request.LastName;
            userInDb.PhoneNumber= request.PhoneNumber;

            var identityResult=await _userManager.UpdateAsync(userInDb);
            if (identityResult.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("User details succesfuly updated.");
            }
            return await ResponseWrapper.FailAsync("Failed to update user details.");
        }
        return await ResponseWrapper.FailAsync("User does not exist.");
    }
}
