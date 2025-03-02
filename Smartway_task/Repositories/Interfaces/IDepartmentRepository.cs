using Smartway_task.Models;

namespace Smartway_task.Repositories.Interfaces;

public interface IDepartmentRepository
{
    public Task<DbDepartment> AddDepartment(DbDepartment department);
    public Task<int?> CheckExistingDepartmentId(int? departmentId);
    public Task<string?> GetDepartmentByPhone(string phone);
}