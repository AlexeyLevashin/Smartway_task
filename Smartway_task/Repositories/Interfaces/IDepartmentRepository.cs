using Smartway_task.Models;

namespace Smartway_task.Repositories.Interfaces;

public interface IDepartmentRepository
{
    public Task<DbDepartment> AddDepartment(DbDepartment department);

    public Task<DbDepartment?> GetDepartmentByPhone(string phone);
    public Task<DbDepartment?> GetDepartmentById(int id);
}