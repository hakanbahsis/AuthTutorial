using Common.Requests.Identity;
using Common.Responses.Wrappers;

namespace Application.Services.Identity;
public interface IUserService
{
    Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request);
    Task<IResponseWrapper> GetUserByIdAsync(Guid userId);
    Task<IResponseWrapper> GetAllUsersAsync();
    Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest request);
    Task<IResponseWrapper> ChangeUserPasswordAsync(ChangePasswordRequest request);
    Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request);
}
