using System.Data;
using Dapper;
using Npgsql;
using Smartway_task.Dapper;
using Smartway_task.Dapper.Interfaces;
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
         var department = await ValidateDepartment(employee.DbDepartment);

        return await _dapperContext.ExecuteInTransaction(async (conn, trans) =>
        {

            var newEmployee = await AddNewEmployee(employee, conn, trans);


            await AddPassport(newEmployee, conn, trans);

            newEmployee.DbDepartment = department; 

            
            return newEmployee;
        });
    }
     
    private async Task<DbEmployee> AddNewEmployee(DbEmployee employee, IDbConnection connection, IDbTransaction transaction)
    {
        var queryObject = new QueryObject(
            @"INSERT INTO Employees (name, surname, phone, companyid, departmentid) 
            VALUES (@name, @surname, @phone, @companyid, @departmentid)
            RETURNING Id, name as ""Name"", surname as ""Surname"", phone as ""Phone"", companyid as ""CompanyId"", departmentid as ""DepartmentId""", 
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
            throw new Exception("Department does not existrdtf.");
        }

        return dbDepartment;
    }
    
     public async Task<DbEmployee?> GetEmployeeByPhone(string phone)
        {
            var queryObject = new QueryObject(
                @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId"",
                         p.id, p.type as ""Type"", p.number as ""Number"", p.employeeid as ""EmployeeId"",
                         d.id, d.name as ""Name"", d.phone as ""Phone""
                  FROM Employees e
                  LEFT JOIN Passports p ON p.EmployeeId = e.Id 
                  LEFT JOIN Departments d ON d.Id = e.DepartmentId
                  WHERE e.phone = @phone",
                new { phone}); 
            
            var employee = await _dapperContext.QueryWithJoin<DbEmployee, DbPassport, DbDepartment, DbEmployee>(
                queryObject,
                (employee, passport, department) =>
                {
                    employee.DbPassport = passport;
                    employee.DbDepartment = department;
                    return employee;
                },
                splitOn: "Id"
            );
            
            return employee.FirstOrDefault();
        }
    
        public async Task<List<DbEmployee>> GetEmployeeByCompanyId(int companyId)
        {
            var queryObject = new QueryObject(
                @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId"",
                         p.id, p.type as ""Type"", p.number as ""Number"", p.employeeid as ""EmployeeId"",
                         d.id, d.name as ""Name"", d.phone as ""Phone""
                  FROM Employees e
                  LEFT JOIN Passports p ON p.EmployeeId = e.Id 
                  LEFT JOIN Departments d ON d.Id = e.DepartmentId
                  WHERE e.companyid = @companyId",
                new {companyId });
        
            return await _dapperContext.QueryWithJoin<DbEmployee, DbPassport, DbDepartment, DbEmployee>(
                queryObject,
                (employee, passport, department) =>
                {
                    employee.DbPassport = passport;
                    employee.DbDepartment = department;
                    return employee;
                },
                splitOn: "Id"
            );
        }
        
        public async Task<List<DbEmployee>> GetEmployeeByDepartmentId(int departmentId)
        {
            var queryObject = new QueryObject(
                @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.companyid as ""CompanyId"", e.departmentid as ""DepartmentId"",
                         p.id, p.type as ""Type"", p.number as ""Number"", p.employeeid as ""EmployeeId"",
                         d.id, d.name as ""Name"", d.phone as ""Phone""
                  FROM Employees e
                  LEFT JOIN Passports p ON p.EmployeeId = e.Id 
                  LEFT JOIN Departments d ON d.Id = e.DepartmentId
                  WHERE e.department = @departmentId",
                new {departmentId });
        
            return await _dapperContext.QueryWithJoin<DbEmployee, DbPassport, DbDepartment, DbEmployee>(
                queryObject,
                (employee, passport, department) =>
                {
                    employee.DbPassport = passport;
                    employee.DbDepartment = department;
                    return employee;
                },
                splitOn: "Id"
            );
        }
    
    private async Task<List<DbEmployee>> GetEmployees(IQueryObject queryObject)
    {
        var dictionary = new Dictionary<int, DbEmployee>(); 
        var res = await _dapperContext.QueryWithJoin<DbEmployee, DbPassport, DbDepartment, DbEmployee>(queryObject, (employee, passport, department) =>
        {
            DbEmployee e;
            if (!dictionary.TryGetValue(employee.Id, out e))
            {
                e = employee;
                e.DbPassport = passport;
                e.DbDepartment = department;
                dictionary.Add(e.Id, e);
            }

            return e;
        }, "Id"); 

        return res.Distinct().ToList(); 
        
    }
}