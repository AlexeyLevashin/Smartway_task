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
            @"INSERT INTO Employees (name, surname, phone, companyid, departmentid) 
            VALUES (@name, @surname, @phone, @companyid, @departmentid)
            RETURNING Id as ""Id"", name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""", 
            new { employee.Name, employee.Surname, employee.Phone, employee.CompanyId, employee.DepartmentId }, 
            transaction);  

        
        var insertedEmployee = await connection.QueryFirstAsync<DbEmployee>(queryObject.Sql, queryObject.Parameters, transaction);

        
        employee.Id = insertedEmployee.Id;

        return employee;
    }


    private async Task AddPassport(DbEmployee employee, IDbConnection connection, IDbTransaction transaction){
   
        var queryObject = new QueryObject(
            @"INSERT INTO Passports (type, number, employeeid) VALUES (@type, @number, @employeeid)",
            new { employee.DbPassport.Type, employee.DbPassport.Number, EmployeeId = employee.Id}, 
            transaction); 

        await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);  
    }

    private async Task<DbDepartment> ValidateDepartment(DbDepartment department)
    {
        var queryObject = new QueryObject(
            @"SELECT * FROM Departments WHERE Id = @Id", new { Id = department.Id });

        var dbDepartment = await _dapperContext.FirstOrDefault<DbDepartment>(queryObject);

        if (dbDepartment == null)
        {
            throw new Exception("Department does not exist.");
        }

        return dbDepartment;
    }
    
     public async Task<DbEmployee?> GetEmployeeByPhone(string phone)
        {
            var queryObject = new QueryObject(
                @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId""
                  FROM Employees e
                  WHERE e.phone = @phone",
                new { phone}); 
            
            return await _dapperContext.FirstOrDefault<DbEmployee>(queryObject);
        }
     
        
        public async Task<List<(DbEmployee, DbPassport, DbDepartment)>> GetEmployeeByDepartmentId(int departmentId)
        {
            var queryObject = new QueryObject(
                @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId"",
                         p.id, p.type as ""Type"", p.number as ""Number"", p.employeeid as ""EmployeeId"",
                         d.id, d.name as ""Name"", d.phone as ""Phone""
                   FROM Employees e
                   LEFT JOIN Passports p ON p.EmployeeId = e.Id 
                   LEFT JOIN Departments d ON d.Id = e.DepartmentId
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
                   FROM Employees e
                   LEFT JOIN Passports p ON p.EmployeeId = e.Id 
                   LEFT JOIN Departments d ON d.Id = e.DepartmentId
                   WHERE e.companyid = @companyId", new {companyId });

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
        public async Task<DbEmployee> UpdateEmployee(DbEmployee employee)
        {
            return await _dapperContext.ExecuteInTransaction<DbEmployee>(async (conn, trans) => 
            {
                var updatedEmployee = await UpdateEmployeeInfo(employee, conn, trans);
                
                await UpdatePassport(employee, conn, trans); 

                return updatedEmployee;
            });
        }
        private async Task<DbEmployee> UpdateEmployeeInfo(DbEmployee dbEmployee, IDbConnection connection, IDbTransaction transaction)
        {
            var queryObject = new QueryObject(
                @"UPDATE employees SET name = @name, surname = @surname, phone = @phone, companyid = @companyid, departmentid = @departmentid
                     WHERE id = @id
                     RETURNING Id as ""Id"", name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""", 
                new {id = dbEmployee.Id, name = dbEmployee.Name, surname = dbEmployee.Surname, phone = dbEmployee.Phone, companyid = dbEmployee.CompanyId, departmentid = dbEmployee.DepartmentId},
                transaction);

            var res = await connection.QueryFirstOrDefaultAsync<DbEmployee>(queryObject.Sql, queryObject.Parameters, transaction);

            return res;
        }
        
    
        private async Task UpdatePassport(DbEmployee employee, IDbConnection connection, IDbTransaction transaction){
   
            var queryObject = new QueryObject(
                @"UPDATE Passports SET type = @type, number = @number
                   WHERE employeeid = @employeeid",
                new { type = employee.DbPassport.Type, number = employee.DbPassport.Number,  employeeid = employee.Id }, 
                transaction); 

            await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);  
        }
        public async Task<DbEmployee?> GetEmployeeById(int id)
        {
            var queryObject = new QueryObject(
                @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId""
                   FROM Employees e
                   WHERE e.id = @id", new {id });
            return await _dapperContext.FirstOrDefault<DbEmployee>(queryObject);
        }
        
    
        public async Task<bool> DeleteEmployee(int employeeId)
        {
            return await _dapperContext.ExecuteInTransaction<bool>(async (conn, trans) => 
            {
                await DeletePassport(employeeId, conn, trans); 
                var deletedEmployee = await DeleteEmployeeInfo(employeeId, conn, trans);

                return deletedEmployee;
            });
        }
        private async Task<bool> DeleteEmployeeInfo(int employeeId, IDbConnection connection, IDbTransaction transaction)
        {
            var queryObject = new QueryObject(
                @"DELETE FROM Employees WHERE id = @id",
                   new {id = employeeId },
                transaction);

            var res = await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);

            return res>0;
        }
        
    
        private async Task DeletePassport(int employeeId, IDbConnection connection, IDbTransaction transaction){
   
            var queryObject = new QueryObject(
                @"DELETE FROM Passports WHERE employeeid = @employeeid",
                new { employeeid = employeeId }, 
                transaction); 

            await connection.ExecuteAsync(queryObject.Sql, queryObject.Parameters, transaction);  
        }
        
}