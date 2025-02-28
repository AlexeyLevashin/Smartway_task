using Smartway_task.DTO;

namespace Smartway_task.Services.Interfaces;

public interface IEmployeeService
{
    public Task<EmployeeResponseDto> AddEmployee(AddNewEmployeeRequestDto addNewEmployeeRequestDto);
    public Task<List<EmployeeResponseDto>> GetEmployeesByCompanyId(int CompanyId);
    public Task<List<EmployeeResponseDto>> GetEmployeesByDepartmentId(int departmentId);
}