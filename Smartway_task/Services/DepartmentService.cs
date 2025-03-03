using Smartway_task.Exceptions;
using Smartway_task.Mappers;
using Smartway_task.Models;
using Smartway_task.DTO.Department.Requests;
using Smartway_task.DTO.Department.Responses;
using Smartway_task.Repositories.Interfaces;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentResponse> AddDepartment(DepartmentRequest departmentRequest)
    {
        if (await _departmentRepository.GetDepartmentByPhone(departmentRequest.Phone) is not null)
        {
            throw new DepartmentIsExistingException("Данный номер телефона отдела уже занят");
        }

        return (await _departmentRepository.AddDepartment(
            new DbDepartment
            {
                Name = departmentRequest.Name,
                Phone = departmentRequest.Phone
            })).MapToDomain().MapToDto();
    }
}