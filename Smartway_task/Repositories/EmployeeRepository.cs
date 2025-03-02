using System.Data;
using Dapper;
using Npgsql;
using Smartway_task.Dapper;
using Smartway_task.Dapper.Interfaces;
using Smartway_task.DTO;
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
    
     public async Task<DbEmployee> AddEmployee(DbEmployee employee)
    {

        return await _dapperContext.ExecuteInTransaction(async (conn, trans) =>
        {

            var newEmployee = await AddNewEmployee(employee, conn, trans);


            await AddPassport(newEmployee, conn, trans);
            
            return newEmployee;
        });
    }
     
    private async Task<DbEmployee> AddNewEmployee(DbEmployee employee, IDbConnection connection, IDbTransaction transaction)
    {
        var queryObject = new QueryObject(
            @"INSERT INTO employees (name, surname, phone, companyid, departmentid) 
                 VALUES (@name, @surname, @phone, @companyid, @departmentid)
                 RETURNING Id as ""Id""", 
            new { employee.Name, employee.Surname, employee.Phone, employee.CompanyId, employee.DepartmentId }, 
            transaction);  

        
        var insertedEmployee = await connection.QueryFirstAsync<DbEmployee>(queryObject.Sql, queryObject.Parameters, transaction);

        
        employee.Id = insertedEmployee.Id;

        return employee;
    }


    private async Task AddPassport(DbEmployee employee, IDbConnection connection, IDbTransaction transaction){
   
        var queryObject = new QueryObject(
            @"INSERT INTO passports (type, number, employeeid) VALUES (@type, @number, @employeeid)",
            new { employee.DbPassport.Type, employee.DbPassport.Number, EmployeeId = employee.Id}, 
            transaction); 

        await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);  
    }
    
     public async Task<DbEmployee?> GetEmployeeByPhone(string phone)
        {
            var queryObject = new QueryObject(
                @"SELECT id, name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""
                     FROM employees
                     WHERE phone = @phone",
                new { phone}); 
            
            return await _dapperContext.FirstOrDefault<DbEmployee>(queryObject);
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
                   WHERE e.departmentid = @departmentId", new {departmentId });

            var employeeResults = await _dapperContext.QueryWithJoin<DbEmployee, DbPassport, DbDepartment, (DbEmployee, DbPassport, DbDepartment)>(
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
                   WHERE e.companyid = @companyId", new {companyId });

            var employeeResults = await _dapperContext.QueryWithJoin<DbEmployee, DbPassport, DbDepartment, (DbEmployee, DbPassport, DbDepartment)>(
                queryObject,
                (employee, passport, department) => (employee, passport, department),
                splitOn: "Id");

            return employeeResults.Select(e => 
            {
                e.Item1.DbPassport = e.Item2;
                return (e.Item1, e.Item2, e.Item3);
            }).ToList();
        }
        public async Task<DbEmployee> UpdateEmployee(DbEmployee employee, int id)
        {
            return await _dapperContext.ExecuteInTransaction<DbEmployee>(async (conn, trans) => 
            {
                var updatedEmployee = await UpdateEmployeeInfo(id, employee, conn, trans);
                
                await UpdatePassport(id, employee, conn, trans); 

                return updatedEmployee;
            });
        }
        private async Task<DbEmployee> UpdateEmployeeInfo(int emplid, DbEmployee dbEmployee, IDbConnection connection, IDbTransaction transaction)
        {
            var queryObject = new QueryObject(
                @"UPDATE employees SET name = @name, surname = @surname, phone = @phone, companyid = @companyid, departmentid = @departmentid
                     WHERE id = @Id
                     RETURNING id as ""Id"", name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""", 
                new {id = emplid, name = dbEmployee.Name, surname = dbEmployee.Surname, phone = dbEmployee.Phone, companyid = dbEmployee.CompanyId, departmentid = dbEmployee.DepartmentId},
                transaction);

            var res = await connection.QueryFirstAsync<DbEmployee>(queryObject.Sql, queryObject.Parameters, transaction);

            return res;
        }
        
    
        private async Task UpdatePassport(int emplid, DbEmployee employee, IDbConnection connection, IDbTransaction transaction){
   
            var queryObject = new QueryObject(
                @"UPDATE passports SET type = @type, number = @number
                   WHERE employeeid = @employeeid",
                new { type = employee.DbPassport.Type, number = employee.DbPassport.Number,  employeeid = emplid }, 
                transaction); 

            await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);  
        }
        public async Task<DbEmployee?> GetEmployeeById(int id)
        {
            var queryObject = new QueryObject(
                @"SELECT id, name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""
                   FROM employees
                   WHERE id = @id", new {id });
            return await _dapperContext.FirstOrDefault<DbEmployee>(queryObject);
        }
        
    
        public async Task DeleteEmployee(int employeeId)
        {
            await _dapperContext.ExecuteInTransaction(async (conn, trans) => 
            {
                await DeletePassport(employeeId, conn, trans); 
                await DeleteEmployeeInfo(employeeId, conn, trans);
            });
        }
        private async Task DeleteEmployeeInfo(int employeeId, IDbConnection connection, IDbTransaction transaction)
        {
            var queryObject = new QueryObject(
                @"DELETE FROM Employees WHERE id = @id",
                   new {id = employeeId },
                transaction);

            await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);
            
        }
        
    
        private async Task DeletePassport(int employeeId, IDbConnection connection, IDbTransaction transaction){
   
            var queryObject = new QueryObject(
                @"DELETE FROM Passports WHERE employeeid = @employeeid",
                new { employeeid = employeeId }, 
                transaction); 

            await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);  
        }
        
}