using Smartway_task.DTO.Department.Requests;
using Smartway_task.DTO.Department.Responses;

namespace Smartway_task.Services.Interfaces;

public interface IDepartmentService
{
    public Task<DepartmentResponse> AddDepartment(DepartmentRequest departmentRequest);
}