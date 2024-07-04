using Application.Services.Identity;
using Common.Requests.Identity;
using Common.Responses.Identity;
using FluentValidation;

namespace Application.Features.Identity.Users.Validators;
public class UserRegistrationRequestValidator:AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator(IUserService userService)
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .MaximumLength(60)
            .EmailAddress()
            .MustAsync(async (email, ct) => await userService.GetUserByEmailAsync(email)
                        is not UserResponse existingUser )
                            .WithMessage("Email is taken.");

        RuleFor(request => request.FirstName)
            .NotEmpty()
            .MaximumLength(60)
            .WithMessage("User firtname is not empty.");
        
        RuleFor(request => request.LastName)
            .NotEmpty()
            .MaximumLength(60)
            .WithMessage("User lastname is not empty.");

        RuleFor(request => request.Email)
            .NotEmpty()
            .MaximumLength(60)
            .WithMessage("User email is not empty.");

        RuleFor(request => request.UserName)
            .NotEmpty()
            .MaximumLength(60)
            .WithMessage("Username is not empty.");

        RuleFor(request => request.Password)
            .NotEmpty();

        RuleFor(request => request.ConfirmPassword)
            .Must((req, confirmed) => req.Password == confirmed)
            .WithMessage("Passwords do not match.");
    }
}
