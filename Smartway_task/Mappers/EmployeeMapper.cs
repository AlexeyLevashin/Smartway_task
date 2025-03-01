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
            Passport = dbEmployee.DbPassport.MapToDomain()
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
            Id = employee.Id,
            Name = employee.Name,
            Surname = employee.Surname,
            Phone = employee.Phone,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            Passport = employee.Passport.MapToDto(),

        };
    }
    public static List<EmployeeWithDepartmentResponseDto> MapToDto(this List<(DbEmployee, DbPassport, DbDepartment)> employees)
    {
        return employees.Select(x => x.Item1.MapToDto(x.Item3)).ToList();
    }

    public static EmployeeWithDepartmentResponseDto MapToDto(this DbEmployee employee, DbDepartment department)
    {
        return new EmployeeWithDepartmentResponseDto
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
    
    


    public static Employee MapToDomain(this EmployeeWithDepartmentResponseDto employeeWithDepartmentResponseDto)
    {
        return new Employee
        {
            Name = employeeWithDepartmentResponseDto.Name,
            Surname = employeeWithDepartmentResponseDto.Surname,
            Phone = employeeWithDepartmentResponseDto.Phone,
            CompanyId = employeeWithDepartmentResponseDto.CompanyId,
            DepartmentId = employeeWithDepartmentResponseDto.DepartmentId,
            Passport = employeeWithDepartmentResponseDto.Passport.MapToDomain()
        };
    }
    
    public static List<Employee> MapToDomain(this List<EmployeeWithDepartmentResponseDto> employeeWithDepartmentResponseDto)
    {
        return employeeWithDepartmentResponseDto.Select(i => i.MapToDomain()).ToList();
    }
    
    
    public static List<EmployeeResponseDto> MapToDto(this List<Employee> employees)
    {
        return employees.Select(i => i.MapToDto()).ToList();
    }



    public static DbEmployee MaptoDb(this Employee employees)
    {
        return new DbEmployee
        {
            Id = employees.Id,
            Name = employees.Name,
            Surname = employees.Surname,
            Phone = employees.Phone,
            CompanyId = employees.CompanyId,
            DepartmentId = employees.DepartmentId,
            DbPassport = employees.Passport.MapToDb()
        };
    }
    
    public static List<DbEmployee> MapToDb(this List<Employee> employees)
    {
        return employees.Select(i => i.MaptoDb()).ToList();
    }
    
    
    
    public static DbEmployee MapToDb(this Employee employee)
    {
        return new DbEmployee
        {
            Id = employee.Id,
            Name = employee.Name,
            Surname = employee.Surname,
            Phone = employee.Phone,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            DbPassport = employee.Passport?.MapToDb() 
        };
    }

    public static DbEmployee ApplyChangesFromDto(this Employee existingDomainEmployee, EmployeeUpdateRequestDto employeeDto)
    {
        DbEmployee dbEmployee = existingDomainEmployee.MapToDb();
        dbEmployee.Name = employeeDto.Name ?? dbEmployee.Name;
        dbEmployee.Surname = employeeDto.Surname ?? dbEmployee.Surname;
        dbEmployee.Phone = employeeDto.Phone ?? dbEmployee.Phone;
        dbEmployee.CompanyId = employeeDto.CompanyId ?? dbEmployee.CompanyId;
        dbEmployee.DepartmentId = employeeDto.DepartmentId ?? dbEmployee.DepartmentId;

        if (employeeDto.Passport != null)
        {
            if (dbEmployee.DbPassport == null)
            {
                dbEmployee.DbPassport = new DbPassport();
            }
            dbEmployee.DbPassport = dbEmployee.DbPassport.ApplyChangesFromDto(employeeDto.Passport); 
        }
        return dbEmployee;
    }

    public static Employee MapToDomainUpd(this DbEmployee dbEmployee)
    {
        return new Employee
        {
            Id = dbEmployee.Id,
            Name = dbEmployee.Name,
            Surname = dbEmployee.Surname,
            Phone = dbEmployee.Phone,
            CompanyId = dbEmployee.CompanyId,
            DepartmentId = dbEmployee.DepartmentId,
            Passport = dbEmployee.DbPassport?.MapToDomain()
        };
    }
    
}