using Common.Requests.Employees;
using FluentValidation;

namespace Application.Features.Employees.Validators;
public class UpdateEmployeeRequestValidator:AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(r=>r.FirstName).NotEmpty().MaximumLength(60);
        RuleFor(r=>r.LastName).NotEmpty().MaximumLength(60);
        RuleFor(r=>r.Email).NotEmpty().MaximumLength(60);
        RuleFor(r=>r.Salary).NotEmpty();
    }
}
