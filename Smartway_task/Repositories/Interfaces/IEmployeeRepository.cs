using Smartway_task.Models;

namespace Smartway_task.Repositories.Interfaces;

public interface IEmployeeRepository
{
    public Task<DbEmployee> AddEmployee(DbEmployee employee);
    public Task<DbEmployee?> GetEmployeeByPhone(string phone);
    public Task<List<DbEmployee>> GetEmployeeByCompanyId(int companyId);
    public Task<List<DbEmployee>> GetEmployeeByDepartmentId(int departmentId);

}