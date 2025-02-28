using Smartway_task.Domain;
using Smartway_task.DTO;
using Smartway_task.Models;

namespace Smartway_task.Mappers;

public static class EmployeeMapper
{
    public static Employee MapToDomain(this DbEmployee dbEmployee)
    {
        return new Employee
        {
            Id = dbEmployee.Id,
            Name = dbEmployee.Name,
            Surname = dbEmployee.Surname,
            Phone = dbEmployee.Phone,
            CompanyId = dbEmployee.CompanyId,
            DepartmentId = dbEmployee.DepartmentId,
            Passport = dbEmployee.DbPassport.MapToDomain(),
            Department = dbEmployee.DbDepartment.MapToDomain(),
        };
    }

    public static List<Employee> MapToDomain(this List<DbEmployee> dbEmployees)
    {
        return dbEmployees.Select(i => i.MapToDomain()).ToList();
    }
    
    public static EmployeeResponseDto MapToDto(this Employee employee)
    {
        return new EmployeeResponseDto
        {
            Name = employee.Name,
            Surname = employee.Surname,
            Phone = employee.Phone,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            Passport = employee.Passport.MapToDto(),
            Department = employee.Department.MapToDto()
        };
    }
    
    public static List<EmployeeResponseDto> MapToDto(this List<Employee> employees)
    {
        return employees.Select(i => i.MapToDto()).ToList();
    }
    
}