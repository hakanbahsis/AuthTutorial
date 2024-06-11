using Application.Services;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Employees.Commands;
public class DeleteEmployeeCommand:IRequest<IResponseWrapper>
{
    public Guid EmplooyeId { get; set; }
}

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, IResponseWrapper>
{
    private readonly IEmployeeService _employeeService;

    public DeleteEmployeeCommandHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IResponseWrapper> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employeeInDb=await _employeeService.GetEmployeeByIdAsync(request.EmplooyeId);
        if (employeeInDb == null)
        {
            var employeeId=await _employeeService.DeleteEmployeeAsync(employeeInDb);
            return  ResponseWrapper<Guid>.Success(employeeId, "Employee entry deleted succcesfuly");      
        }
        return await ResponseWrapper.FailAsync("Employee does not exist.");
    }
}
