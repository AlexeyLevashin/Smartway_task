using Smartway_task.Domain;
using Smartway_task.DTO;
using Smartway_task.Models;

namespace Smartway_task.Mappers;

public static class DepartmentMapper
{
    public static Department MapToDomain(this AddNewDepartmentRequestDto addNewDepartmentRequestDto)
    {
        return new Department
        {
            Name = addNewDepartmentRequestDto.Name,
            Phone = addNewDepartmentRequestDto.Phone
        };
    }

    public static Department MapToDomain(this AddDepartmentRequestDto addDepartmentRequestDto)
    {
        return  new Department{};
    }
    
    public static Department MapToDomain(this DbDepartment dbDepartment)
    {
        return new Department
        {
            Id = dbDepartment.Id,
            Name = dbDepartment.Name,
            Phone = dbDepartment.Phone
        };
    }
    
    public static DepartmentResponseDto MapToDto(this Department department)
    {
        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Phone = department.Phone
        };
    }
    
    
    
    public static DbDepartment MapToDb(this Department department)
    {
        return new DbDepartment
        {
            Id = department.Id,
            Name = department.Name,
            Phone = department.Phone
        };
    }
}