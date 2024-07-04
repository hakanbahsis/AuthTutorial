using Application.Services;
using Common.Requests.Employees;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Employees.Validators;
public class UpdateEmployeeRequestValidator:AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator(IEmployeeService employeeService)
    {
        RuleFor(request=>request.Id)
            .MustAsync(async(id,ct)=>await employeeService.GetEmployeeByIdAsync(id) 
            is Employee employeeInDb && employeeInDb.Id==id)
            .WithMessage("Employee does not exist.");

        RuleFor(r=>r.FirstName)
            .NotEmpty()
            .WithMessage("Employee firstname is required.")
            .MaximumLength(60);

        RuleFor(r=>r.LastName)
            .NotEmpty()
            .WithMessage("Employee firstname is required.")
            .MaximumLength(60);

        RuleFor(r=>r.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Employee email is required.")
            .MaximumLength(60);

        RuleFor(r=>r.Salary)
            .NotEmpty()
            .WithMessage("Employee must have a salary.");
    }
}
