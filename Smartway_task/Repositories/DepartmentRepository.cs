using Smartway_task.Dapper;
using Smartway_task.Dapper.Interfaces;
using Smartway_task.Models;
using Smartway_task.Repositories.Interfaces;

namespace Smartway_task.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDapperContext _dapperContext;

    public DepartmentRepository(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<DbDepartment> AddDepartment(DbDepartment department)
    {
        var queryObject = new QueryObject(
            @"INSERT INTO departments (Name, Phone) VALUES (@Name, @Phone)
                RETURNING Id, Name, Phone", new { department.Name, department.Phone });

        return await _dapperContext.CommandWithResponse<DbDepartment>(queryObject);
    }

    public async Task<int?> CheckExistingDepartmentId(int? departmentId)
    {
        var queryObject = new QueryObject(
            @"SELECT 1 FROM departments WHERE id = @Id LIMIT 1",
            new {id = departmentId });
        return await _dapperContext.FirstOrDefault<int?>(queryObject);
    }

    public async Task<string?> GetDepartmentByPhone(string phone)
    {
        var queryObject = new QueryObject(
            @"SELECT 1 FROM departments WHERE id = @Id LIMIT 1",
            new { phone });
        return await _dapperContext.FirstOrDefault<string>(queryObject);
    }
}