using System.Data;
using Smartway_task.Models;

namespace Smartway_task.Repositories.Interfaces;

public interface IPassportRepository
{
    public Task AddPassport(DbEmployee employee, IDbTransaction? transaction = null);
    
    public Task DeletePassport(int employeeId, IDbTransaction? transaction = null);
    public Task UpdatePassport(int emplid, DbEmployee dbEmployee, IDbTransaction? transaction = null);
    public Task<string?> GetEmployeeByPassportNumber(string? number);
    public Task<int> GetEmployeeIdByPasportNumber(string? number);
}