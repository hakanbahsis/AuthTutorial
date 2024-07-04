using Common.Requests.Employees;
using FluentValidation;

namespace Application.Features.Employees.Validators;
public class CreateEmployeeRequestValidator:AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(r => r.FirstName)
            .NotEmpty()
            .WithMessage("Employee firstname is required.")
            .MaximumLength(60);

        RuleFor(r => r.LastName)
            .NotEmpty()
            .WithMessage("Employee firstname is required.")
            .MaximumLength(60);

        RuleFor(r => r.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Employee email is required.")
            .MaximumLength(60);

        RuleFor(r => r.Salary)
            .NotEmpty()
            .WithMessage("Employee must have a salary.");
    }
}
