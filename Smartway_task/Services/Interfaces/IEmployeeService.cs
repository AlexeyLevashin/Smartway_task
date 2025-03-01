using Smartway_task.DTO;
using Smartway_task.Models;

namespace Smartway_task.Services.Interfaces;

public interface IEmployeeService
{
    public Task<EmployeeResponseDto> AddEmployee(AddNewEmployeeRequestDto addNewEmployeeRequestDto);
    public Task<List<EmployeeWithDepartmentResponseDto>> GetEmployeesByCompanyId(int CompanyId);
    public Task<List<EmployeeWithDepartmentResponseDto>> GetEmployeesByDepartmentId(int departmentId);
    public Task<DbEmployee?> UpdateEmployee(EmployeeUpdateRequestDto employeeDto);
    public Task DeleteEmployee(int id);
}