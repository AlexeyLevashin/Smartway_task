using Smartway_task.Models;

namespace Smartway_task.Repositories.Interfaces;

public interface IEmployeeRepository
{
    public Task<DbEmployee> AddEmployee(DbEmployee employee);
    public Task<DbEmployee?> GetEmployeeByPhone(string phone);
    public Task<DbEmployee?> GetEmployeeById(int id);
    public Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByCompanyId(int companyId);
    public Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByDepartmentId(int departmentId);
    public Task<DbEmployee> UpdateEmployee(DbEmployee employee, int id);
    public Task DeleteEmployee(int employeeId);


}