using Smartway_task.Models;
using Smartway_task.DTO.Employee.Requests;
using Smartway_task.DTO.Employee.Responses;

namespace Smartway_task.Services.Interfaces;

public interface IEmployeeService
{
    public Task<EmployeeResponse> AddEmployee(AddNewEmployeeRequest addNewEmployeeRequest);
    public Task<List<EmployeeWithDepartmentResponse>> GetEmployeesByCompanyId(int сompanyId);
    public Task<List<EmployeeWithDepartmentResponse>> GetEmployeesByDepartmentId(int departmentId);
    public Task<EmployeeResponse> UpdateEmployee(EmployeeUpdateRequest employeeDto, int id);
    
    public Task DeleteEmployee(int id);
}