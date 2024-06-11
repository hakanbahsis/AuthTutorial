using Application.Services;
using AutoMapper;
using Common.Requests.Employees;
using Common.Responses.Employees;
using Common.Responses.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Employees.Commands;
public class CreateEmployeeCommand:IRequest<IResponseWrapper>
{
    public CreateEmployeeRequest CreateEmployeeRequest { get; set; }

}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, IResponseWrapper>
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    public CreateEmployeeCommandHandler(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    public async Task<IResponseWrapper> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var mappedEmployee=_mapper.Map<Employee>(request.CreateEmployeeRequest);
        var newEmployee=await _employeeService.CreateEmployeeAsync(mappedEmployee);
        if (newEmployee != null)
        {
           var  mappedNewEmployee=_mapper.Map<EmployeeResponse>(newEmployee);
            return await ResponseWrapper<EmployeeResponse>.SuccessAsync(mappedNewEmployee,"Employee created succesfuly.");
        }
        return await ResponseWrapper.FailAsync("Failed to create employee entry.");
    }
}
