using Smartway_task.DTO;
using Smartway_task.Exceptions;
using Smartway_task.Mappers;
using Smartway_task.Models;
using Smartway_task.Repositories.Interfaces;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Services;

public class DepartmentService:IDepartmentService
{
    public IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentResponseDto> AddDepartment(AddNewDepartmentRequestDto addNewDepartmentRequestDto)
    {
        if (await _departmentRepository.GetDepartmentByPhone(addNewDepartmentRequestDto.Phone) is not null)
        {
            throw new DepartmentIsExistingException("Данный отдел уже существует");
        }

        return (await _departmentRepository.AddDepartment(
            new DbDepartment
            {
                Name = addNewDepartmentRequestDto.Name,
                Phone = addNewDepartmentRequestDto.Phone
            })).MapToDomain().MapToDto();
    }
}