using System.Data;
using Dapper;
using Smartway_task.Dapper;
using Smartway_task.Dapper.Interfaces;
using Smartway_task.Models;
using Smartway_task.Repositories.Interfaces;

namespace Smartway_task.Repositories;

public class PassportRepository : IPassportRepository
{
    private readonly IDapperContext _dapperContext;

    public PassportRepository(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task AddPassport(DbEmployee employee, IDbTransaction? transaction = null)
    {
        var queryObject = new QueryObject(
            @"INSERT INTO passports (type, number, employeeid) VALUES (@type, @number, @employeeid)",
            new { employee.DbPassport.Type, employee.DbPassport.Number, EmployeeId = employee.Id });

        await _dapperContext.Command(queryObject, transaction);
    }

    public async Task DeletePassport(int employeeId, IDbTransaction? transaction = null)
    {
        var queryObject = new QueryObject(
            @"DELETE FROM passports WHERE employeeid = @employeeid",
            new { employeeid = employeeId });

        await _dapperContext.Command(queryObject, transaction);
    }

    public async Task UpdatePassport(int emplid, DbEmployee employee, IDbTransaction? transaction = null)
    {
        var queryObject = new QueryObject(
            @"UPDATE passports SET type = @type, number = @number
                 WHERE employeeid = @employeeid",
            new { type = employee.DbPassport.Type, number = employee.DbPassport.Number, employeeid = emplid });

        await _dapperContext.Command(queryObject, transaction);
    }

    public async Task<string?> GetEmployeeByPassportNumber(string? number)
    {
        var queryObject = new QueryObject(
            @"SELECT 1 FROM passports WHERE number = @number LIMIT 1",
            new {Number = number });
        return await _dapperContext.FirstOrDefault<string>(queryObject);
        
    }

    public async Task<int> GetEmployeeIdByPasportNumber(string? number)
    {
        var queryObject = new QueryObject(
            @"SELECT employeeid FROM passports WHERE number = @number LIMIT 1",
            new { number });

        return await _dapperContext.FirstOrDefault<int>(queryObject);
    }
}