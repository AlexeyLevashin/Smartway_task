using System.Data;
using Dapper;
using Npgsql;
using Smartway_task.Dapper;
using Smartway_task.Dapper.Interfaces;
using Smartway_task.Mappers;
using Smartway_task.Models;
using Smartway_task.Repositories.Interfaces;
using Smartway_task.Scripts.Employees;

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
            PostgresEmployeeElement.AddEmployee,
            new { employee.Name, employee.Surname, employee.Phone, employee.CompanyId, employee.DepartmentId }
        );

        var insertedEmployee = await _dapperContext.CommandWithResponse<DbEmployee>(queryObject, transaction);
        employee.Id = insertedEmployee.Id;

        return employee;
    }


    public async Task<string?> GetEmployeeByPhone(string? phone)
    {
        var queryObject = new QueryObject(
            PostgresEmployeeElement.GetEmployeeByPhone,
            new { phone });

        return await _dapperContext.FirstOrDefault<string>(queryObject);
    }

    public async Task<int> GetEmployeeIdByPhone(string? phone)
    {
        var queryObject = new QueryObject(
            PostgresEmployeeElement.GetEmployeeIdByPhone,
            new { phone });

        return await _dapperContext.FirstOrDefault<int>(queryObject);
    }
    
    public async Task<int?> CheckExistingCompanyId(int companyId)
    {
        var queryObject = new QueryObject(
            PostgresEmployeeElement.CheckExistingCompanyId,
            new { companyid = companyId });
        return await _dapperContext.FirstOrDefault<int?>(queryObject);
    }


    public async Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByDepartmentId(int departmentId)
    {
        var queryObject = new QueryObject(
            PostgresEmployeeElement.GetEmployeeByDepartmentId, new { departmentId });

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
            PostgresEmployeeElement.GetEmployeeByCompanyId, new { companyId });

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
            PostgresEmployeeElement.UpdateEmployee,
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
            PostgresEmployeeElement.GetEmployeeById, new { id });
        return await _dapperContext.FirstOrDefault<DbEmployee>(queryObject);
    }


    public async Task DeleteEmployee(int employeeId, IDbTransaction? transaction = null)
    {
        var queryObject = new QueryObject(
            PostgresEmployeeElement.DeleteEmployee,
            new { id = employeeId });

        await _dapperContext.Command(queryObject, transaction);
    }
}