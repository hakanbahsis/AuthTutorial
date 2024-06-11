using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;
public interface IEmployeeService
{
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<Employee> UpdateEmployeeAsync(Employee employee);
    Task<Guid> DeleteEmployeeAsync(Employee employee);
    Task<Employee> GetEmployeeByIdAsync(Guid id);
    Task<List<Employee>> GetAllEmployeesAsync();
}
