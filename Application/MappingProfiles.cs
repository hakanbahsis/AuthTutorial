using AutoMapper;
using Common.Requests.Employees;
using Common.Responses.Employees;
using Common.Responses.Identity;
using Domain.Entities;

namespace Application;
public class MappingProfiles:Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateEmployeeRequest, Employee>().ReverseMap();
        CreateMap<UpdateEmployeeRequest, Employee>().ReverseMap();
        CreateMap<Employee,EmployeeResponse>().ReverseMap();
       
    }
}
