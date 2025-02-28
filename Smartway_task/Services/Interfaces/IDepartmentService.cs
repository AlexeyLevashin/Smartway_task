using Smartway_task.DTO;

namespace Smartway_task.Services.Interfaces;

public interface IDepartmentService
{
    public Task<DepartmentResponseDto> AddDepartment(AddNewDepartmentRequestDto addNewDepartmentRequestDto);
}