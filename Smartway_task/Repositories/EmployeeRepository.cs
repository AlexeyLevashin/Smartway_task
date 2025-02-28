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
            @"INSERT INTO Employees (Name, Surname, Phone, CompanyId, DepartmentId) 
            VALUES (@Name, @Surname, @Phone, @CompanyId, @DepartmentId)
            RETURNING Id, Name, Surname, Phone, CompanyId, DepartmentId", 
            new { employee.Name, employee.Surname, employee.Phone, employee.CompanyId, DepartmentId = employee.DbDepartment.Id }, 
            transaction);  

        
        var insertedEmployee = await connection.QueryFirstAsync<DbEmployee>(queryObject.Sql, queryObject.Parameters, transaction);

        
        employee.Id = insertedEmployee.Id;

        return employee;
    }


    private async Task AddPassport(DbEmployee employee, IDbConnection connection, IDbTransaction transaction)
    {
        var queryObject = new QueryObject(
            @"INSERT INTO Passports (Type, Number, EmployeeId) VALUES (@Type, @Number, @EmployeeId)",
            new { employee.DbPassport.Type, employee.DbPassport.Number, EmployeeId = employee.Id }, 
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
                @"SELECT e.id as Id, e.name as Name, e.surname as Surname, e.phone as Phone, e.company_id as CompanyId,
                         p.id as PassportId, p.type as Type, p.number as Number,
                         d.id as DepartmentId, d.name as DepartmentName, d.phone as DepartmentPhone
                  FROM Employees e
                  LEFT JOIN Passports p ON e.Id = p.EmployeeId  
                  LEFT JOIN Departments d ON e.DepartmentId = d.Id  
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
                splitOn: "PassportId,DepartmentId"
            );
    
            return employee.FirstOrDefault();
        }
    
        public async Task<List<DbEmployee>> GetEmployeeByIdCompany(int companyId)
        {
            var queryObject = new QueryObject(
                @"SELECT e.id as Id, e.name as Name, e.surname as Surname, e.phone as Phone, e.company_id as CompanyId,
                         p.id as PassportId, p.type as Type, p.number as Number,
                         d.id as DepartmentId, d.name as DepartmentName, d.phone as DepartmentPhone
                  FROM Employees e
                  LEFT JOIN Passports p ON p.EmployeeId = e.Id 
                  LEFT JOIN Departments d ON d.Id = e.DepartmentId
                  WHERE e.company_id = @company_id",
                new { company_id = companyId });
    
            return await _dapperContext.QueryWithJoin<DbEmployee, DbPassport, DbDepartment, DbEmployee>(
                queryObject,
                (employee, passport, department) =>
                {
                    employee.DbPassport = passport;
                    employee.DbDepartment = department;
                    return employee;
                },
                splitOn: "PassportId,DepartmentId"
            );
        }
    
 // public async Task<DbEmployee> AddEmployee(DbEmployee employee)
 //    {
 //        return await _dapperContext.ExecuteInTransaction<DbEmployee>(async (conn, trans) =>
 //        {
 //            var department = await ValidateDepartment(employee.DbDepartment);
 //
 //            employee = await AddEmployee(employee, trans);
 //            
 //            await AddPassport(employee, trans);
 //
 //            employee.DbDepartment = department;
 //
 //            return employee;
 //        });
 //    }
 //
 //    private async Task<DbEmployee> AddEmployee(DbEmployee employee, IDbTransaction transaction)
 //    {
 //        var queryObject = new QueryObject(
 //            @"INSERT INTO Employees (Name, Surname, Phone, CompanyId) VALUES (@Name, @Surname, @Phone, @CompanyId)
 //            RETURNING Id", new { employee.Name, employee.Surname, employee.Phone, employee.CompanyId }, transaction);
 //
 //        return await _dapperContext.CommandWithResponse<DbEmployee>(queryObject);
 //    }
 //
 //    private async Task AddPassport(DbEmployee employee, IDbTransaction transaction)
 //    {
 //        var queryObject = new QueryObject(
 //        @"INSERT INTO Passports (Type, Number, EmployeeId) VALUES (@Type, @Number, @EmployeeId)
 //            RETURNING Id", new { employee.DbPassport.Type, employee.DbPassport.Number, EmployeeId = employee.Id }, transaction);
 //
 //        var res = await _dapperContext.CommandWithResponse<DbPassport>(queryObject);
 //    }
 //    
 //    private async Task<DbDepartment> ValidateDepartment(DbDepartment department)
 //    {
 //        var queryObject = new QueryObject(
 //            @"SELECT * FROM Departments WHERE Id = @Id", new { department.Id });
 //
 //        var dbDepartment = await _dapperContext.FirstOrDefault<DbDepartment>(queryObject);
 //
 //        if (dbDepartment == null)
 //        {
 //            throw new Exception("Department does not exist.");
 //        }
 //
 //        return dbDepartment;
 //    }

    // public async Task<DbEmployee?> GetEmployeeByPhone(string phone)
    // {
    //     var queryObject = new QueryObject(
    //         @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.company_id as ""CompanyId"",
    //                     p.id, p.type as ""Type"", p.number as ""Number"",
    //                     d.id, d.name as ""Name"", d.phone as ""Phone""
    //             FROM Employees e
    //             LEFT JOIN passports p ON e.passport_id = p.id  
    //             LEFT JOIN departments d ON e.department_id = d.id 
    //             WHERE e.phone = @phone", new {phone});
    //     return await _dapperContext.FirstOrDefault<DbEmployee>(queryObject);
    // }
    //
    // public async Task<List<DbEmployee>> GetEmployeeByIdCompany(int companyId)
    // {
    //     var queryObject = new QueryObject(
    //         @"SELECT e.id, e.name as ""Name"", e.surname as ""Surname"", e.phone as ""Phone"", e.company_id as ""CompanyId"",
    //                    /* p.id,*/ p.type as ""Type"", p.number as ""Number"",
    //                     /*d.id,*/ d.name as ""Name"", d.phone as ""Phone""
    //             FROM Employees e
    //             LEFT JOIN passport p ON p.employee_id = e.id
    //             LEFT JOIN departments d ON d.id =  
    //             WHERE e.company_id = companyId", new {company_id = companyId } );
    //     return await GetEmployees(queryObject);
    // }
    
    
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
        }, "PassportType, DepartmentName"); 

        return res.Distinct().ToList(); 
        
    }
}