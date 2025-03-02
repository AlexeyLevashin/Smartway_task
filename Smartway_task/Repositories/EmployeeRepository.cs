using System.Data;
using Dapper;
using Npgsql;
using Smartway_task.Dapper;
using Smartway_task.Dapper.Interfaces;
using Smartway_task.Mappers;
using Smartway_task.Models;
using Smartway_task.Repositories.Interfaces;

namespace Smartway_task.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDapperContext _dapperContext;

    public EmployeeRepository(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IDbTransaction> BeginTransaction()
    {
        return await _dapperContext.BeginTransaction();
    }

    public async Task<DbEmployee> AddEmployee(DbEmployee employee, IDbTransaction? transaction = null)
    {
        var queryObject = new QueryObject(
            @"INSERT INTO employees (name, surname, phone, companyid, departmentid) 
                 VALUES (@name, @surname, @phone, @companyid, @departmentid)
                 RETURNING Id as ""Id""",
            new { employee.Name, employee.Surname, employee.Phone, employee.CompanyId, employee.DepartmentId }
        );

        var insertedEmployee = await _dapperContext.CommandWithResponse<DbEmployee>(queryObject, transaction);
        employee.Id = insertedEmployee.Id;

        return employee;
    }


    public async Task<string?> GetEmployeeByPhone(string? phone)
    {
        var queryObject = new QueryObject(
            @"SELECT 1 FROM employees WHERE phone = @phone LIMIT 1",
            new { phone });

        return await _dapperContext.FirstOrDefault<string>(queryObject);
    }

    public async Task<int> GetEmployeeIdByPhone(string? phone)
    {
        var queryObject = new QueryObject(
            @"SELECT id FROM employees WHERE phone = @phone LIMIT 1",
            new { phone });

        return await _dapperContext.FirstOrDefault<int>(queryObject);
    }
    
    public async Task<int?> CheckExistingCompanyId(int companyId)
    {
        var queryObject = new QueryObject(
            @"SELECT 1 FROM employees WHERE companyid = @companyId LIMIT 1",
            new { companyid = companyId });
        return await _dapperContext.FirstOrDefault<int?>(queryObject);
    }


    public async Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByDepartmentId(int departmentId)
    {
        var queryObject = new QueryObject(
            @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId"",
                         p.id, p.type as ""Type"", p.number as ""Number"", p.employeeid as ""EmployeeId"",
                         d.id, d.name as ""Name"", d.phone as ""Phone""
                    FROM employees e
                   LEFT JOIN passports p ON p.EmployeeId = e.Id 
                   LEFT JOIN departments d ON d.Id = e.DepartmentId
                   WHERE e.departmentid = @departmentId", new { departmentId });

        var employeeResults = await _dapperContext
            .QueryWithJoin<DbEmployee, DbPassport, DbDepartment, (DbEmployee, DbPassport, DbDepartment)>(
                queryObject,
                (employee, passport, department) => (employee, passport, department),
                splitOn: "Id");

        return employeeResults.Select(x =>
        {
            x.Item1.DbPassport = x.Item2;
            return (x.Item1, x.Item2, x.Item3);
        }).ToList();
    }


    public async Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByCompanyId(int companyId)
    {
        var queryObject = new QueryObject(
            @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId"",
                         p.id, p.type as ""Type"", p.number as ""Number"", p.employeeid as ""EmployeeId"",
                         d.id, d.name as ""Name"", d.phone as ""Phone""
                   FROM employees e
                   LEFT JOIN passports p ON p.EmployeeId = e.Id 
                   LEFT JOIN departments d ON d.Id = e.DepartmentId
                   WHERE e.companyid = @companyId", new { companyId });

        var employeeResults = await _dapperContext
            .QueryWithJoin<DbEmployee, DbPassport, DbDepartment, (DbEmployee, DbPassport, DbDepartment)>(
                queryObject,
                (employee, passport, department) => (employee, passport, department),
                splitOn: "Id");

        return employeeResults.Select(e =>
        {
            e.Item1.DbPassport = e.Item2;
            return (e.Item1, e.Item2, e.Item3);
        }).ToList();
    }

    public async Task<DbEmployee> UpdateEmployee(int emplid, DbEmployee dbEmployee, IDbTransaction? transaction = null)
    {
        var queryObject = new QueryObject(
            @"UPDATE employees SET name = @name, surname = @surname, phone = @phone, companyid = @companyid, departmentid = @departmentid
                     WHERE id = @Id
                     RETURNING id as ""Id"", name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""",
            new
            {
                id = emplid, name = dbEmployee.Name, surname = dbEmployee.Surname, phone = dbEmployee.Phone,
                companyid = dbEmployee.CompanyId, departmentid = dbEmployee.DepartmentId
            });

        var res = await _dapperContext.CommandWithResponse<DbEmployee>(queryObject, transaction);

        return res;
    }


    public async Task<DbEmployee?> GetEmployeeById(int id)
    {
        var queryObject = new QueryObject(
            @"SELECT id, name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""
                   FROM employees
                   WHERE id = @id", new { id });
        return await _dapperContext.FirstOrDefault<DbEmployee>(queryObject);
    }


    public async Task DeleteEmployee(int employeeId, IDbTransaction? transaction = null)
    {
        var queryObject = new QueryObject(
            @"DELETE FROM Employees WHERE id = @id",
            new { id = employeeId });

        await _dapperContext.Command(queryObject, transaction);
    }
}