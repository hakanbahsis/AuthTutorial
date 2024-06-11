using Application.Services;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _appDbContext;

    public EmployeeService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        await _appDbContext.Employees.AddAsync(employee);
        await _appDbContext.SaveChangesAsync();
        return employee;
    }

    public async Task<Guid> DeleteEmployeeAsync(Employee employee)
    {
        _appDbContext.Employees.Remove(employee);
        await _appDbContext.SaveChangesAsync();
        return employee.Id;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _appDbContext.Employees.ToListAsync();
    }

    public async Task<Employee> GetEmployeeByIdAsync(Guid id)
    {
        var employeeInDb=await _appDbContext.Employees.Where(emp=>emp.Id==id).FirstOrDefaultAsync();
        return employeeInDb;
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
         _appDbContext.Employees.Update(employee);
        await _appDbContext.SaveChangesAsync();
        return employee;
    }
}
