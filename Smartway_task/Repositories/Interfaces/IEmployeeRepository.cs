using System.Data;
using Smartway_task.Models;

namespace Smartway_task.Repositories.Interfaces;

public interface IEmployeeRepository
{
    public Task<IDbTransaction> BeginTransaction();
    public Task<DbEmployee> AddEmployee(DbEmployee employee, IDbTransaction? transaction = null);
    public Task<DbEmployee?> GetEmployeeByPhone(string phone);
    public Task<DbEmployee?> GetEmployeeById(int id);
    public Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByCompanyId(int companyId);
    public Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByDepartmentId(int departmentId);
    public Task<DbEmployee> UpdateEmployee(int emplid, DbEmployee dbEmployee, IDbTransaction? transaction = null);
    public Task DeleteEmployee(int employeeId, IDbTransaction? transaction = null);


}