using Application.Features.Employees.Commands;
using FluentValidation;

namespace Application.Features.Employees.Validators;
public class UpdateEmployeeCommandValidator:AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(c=>c.UpdateEmployeeRequest).SetValidator(new UpdateEmployeeRequestValidator());
    }
}
