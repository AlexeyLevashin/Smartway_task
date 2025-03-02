using Smartway_task.DTO;
using Smartway_task.Models;
using Smartway_task.NewDto.Employee.Requests;
using Smartway_task.NewDto.Employee.Responses;

namespace Smartway_task.Services.Interfaces;

public interface IEmployeeService
{
    public Task<EmployeeResponse> AddEmployee(AddNewEmployeeRequest addNewEmployeeRequest);
    public Task<List<EmployeeWithDepartmentResponse>> GetEmployeesByCompanyId(int сompanyId);
    public Task<List<EmployeeWithDepartmentResponse>> GetEmployeesByDepartmentId(int departmentId);
    // public Task<DbEmployee> UpdateEmployee(EmployeeUpdateRequestDto employeeDto, int id);
    public Task<DbEmployee> UpdateEmployee(EmployeeUpdateRequest employeeDto, int id);
    
    public Task DeleteEmployee(int id);
}