using Application.Features.Employees.Commands;
using Application.Services;
using Application.Services.Identity;
using FluentValidation;

namespace Application.Features.Employees.Validators;
public class UpdateEmployeeCommandValidator:AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator(IEmployeeService employeeService)
    {
        RuleFor(c=>c.UpdateEmployeeRequest).SetValidator(new UpdateEmployeeRequestValidator(employeeService));
    }
}
