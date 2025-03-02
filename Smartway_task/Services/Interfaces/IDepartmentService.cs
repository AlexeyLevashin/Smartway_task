using Smartway_task.NewDto.Department.Requests;
using Smartway_task.NewDto.Department.Responses;

namespace Smartway_task.Services.Interfaces;

public interface IDepartmentService
{
    public Task<DepartmentResponse> AddDepartment(DepartmentRequest departmentRequest);
}