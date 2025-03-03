using Smartway_task.Models;
using Smartway_task.DTO.Employee.Requests;
using Smartway_task.DTO.Employee.Responses;

namespace Smartway_task.Mappers;

public static class EmployeeMapper
{
    public static DbEmployee MapToDb(this AddNewEmployeeRequest dto)
    {
        return new DbEmployee
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Phone = dto.Phone,
            CompanyId = dto.CompanyId,
            DepartmentId = dto.DepartmentId,
        };
    }

    public static EmployeeResponse MapToDto(this DbEmployee dbEmployee)
    {
        return new EmployeeResponse
        {
            Id = dbEmployee.Id,
            Name = dbEmployee.Name,
            Surname = dbEmployee.Surname,
            Phone = dbEmployee.Phone,
            CompanyId = dbEmployee.CompanyId,
            DepartmentId = dbEmployee.DepartmentId,
            Passport = dbEmployee.DbPassport.MapToDto()
        };
    }

    public static List<EmployeeWithDepartmentResponse> MapToDto(
        this List<(DbEmployee, DbPassport, DbDepartment)> employees)
    {
        return employees.Select(x => x.Item1.MapToDto(x.Item3)).ToList();
    }

    public static EmployeeWithDepartmentResponse MapToDto(this DbEmployee employee, DbDepartment department)
    {
        return new EmployeeWithDepartmentResponse
        {
            Id = employee.Id,
            Name = employee.Name,
            Surname = employee.Surname,
            Phone = employee.Phone,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            Passport = employee.DbPassport.MapToDomain().MapToDto(),
            Department = department.MapToDomain().MapToDto()
        };
    }


    public static DbEmployee ApplyChangesFromDto(this DbEmployee existingDbEmployee, EmployeeUpdateRequest employeeDto)
    {
        existingDbEmployee.Name = employeeDto.Name ?? existingDbEmployee.Name;
        existingDbEmployee.Surname = employeeDto.Surname ?? existingDbEmployee.Surname;
        existingDbEmployee.Phone = employeeDto.Phone ?? existingDbEmployee.Phone;
        existingDbEmployee.CompanyId = employeeDto.CompanyId ?? existingDbEmployee.CompanyId;
        existingDbEmployee.DepartmentId = employeeDto.DepartmentId ?? existingDbEmployee.DepartmentId;

        if (employeeDto.Passport != null)
        {
            if (existingDbEmployee.DbPassport == null)
            {
                existingDbEmployee.DbPassport = new DbPassport();
            }

            existingDbEmployee.DbPassport.ApplyChangesFromDto(employeeDto.Passport);
        }

        return existingDbEmployee;
    }
}