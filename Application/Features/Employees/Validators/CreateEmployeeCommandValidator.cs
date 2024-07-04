using Application.Features.Employees.Commands;
using FluentValidation;

namespace Application.Features.Employees.Validators;
public class CreateEmployeeCommandValidator:AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(c => c.CreateEmployeeRequest)
            .SetValidator(new CreateEmployeeRequestValidator());
    }
}
