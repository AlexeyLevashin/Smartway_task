using Smartway_task.Domain;
using Smartway_task.Models;
using Smartway_task.NewDto.Department.Responses;

namespace Smartway_task.Mappers;

public static class NewDepartmentMapper
{
    public static Department MapToDomain(this DbDepartment dbDepartment)
    {
        return new Department
        {
            Id = dbDepartment.Id,
            Name = dbDepartment.Name,
            Phone = dbDepartment.Phone
        };
    }
    
    public static DepartmentResponse MapToDto(this Department department)
    {
        return new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
            Phone = department.Phone
        };
    }
}