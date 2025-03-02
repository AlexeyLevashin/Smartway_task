using Smartway_task.Exceptions;
using Smartway_task.Mappers;
using Smartway_task.Models;
using Smartway_task.NewDto.Employee.Requests;
using Smartway_task.NewDto.Employee.Responses;
using Smartway_task.Repositories.Interfaces;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPassportRepository _passportRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IPassportRepository passportRepository)
    {
        _employeeRepository = employeeRepository;
        _passportRepository = passportRepository;
    }


    public async Task<EmployeeResponse> AddEmployee(AddNewEmployeeRequest addNewEmployeeRequest)
    {
        if (await _employeeRepository.GetEmployeeByPhone(addNewEmployeeRequest.Phone) is not null)
        {
            throw new PhoneIsExistingException("Сотрудник с данным номером телефона уже существует");
        }

        DbEmployee dbEmployee = addNewEmployeeRequest.MapToDb();

        dbEmployee.DbPassport = addNewEmployeeRequest.NewPassport.MapToDb();

        var transaction = await _employeeRepository.BeginTransaction();
        try
        {
            await _employeeRepository.AddEmployee(dbEmployee, transaction);
            await _passportRepository.AddPassport(dbEmployee, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

        EmployeeResponse employeeResponse = dbEmployee.MapToDto();

        return employeeResponse;
    }

    public async Task<List<EmployeeWithDepartmentResponse>> GetEmployeesByCompanyId(int companyId)
    {
        var res = await _employeeRepository.GetEmployeeByCompanyId(companyId);
        return res.MapToDto();
    }

    public async Task<List<EmployeeWithDepartmentResponse>> GetEmployeesByDepartmentId(int departmentId)
    {
        var res = await _employeeRepository.GetEmployeeByDepartmentId(departmentId);
        return res.MapToDto();
    }


    public async Task<EmployeeResponse> UpdateEmployee(EmployeeUpdateRequest employeeUpdateRequest, int id)
    {
        var existingEmployeeDb = await _employeeRepository.GetEmployeeById(id);
        if (existingEmployeeDb is null)
        {
            throw new EmployeeIsNotExistingException("Сотрудник с данным id не существует");
        }


        var dbEmployee = existingEmployeeDb.ApplyChangesFromDto(employeeUpdateRequest);


        var transaction = await _employeeRepository.BeginTransaction();
        try
        {
            await _employeeRepository.UpdateEmployee(id, dbEmployee, transaction);
            await _passportRepository.UpdatePassport(id, dbEmployee, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

        return dbEmployee.MapToDto();
    }


    public async Task DeleteEmployee(int id)
    {
        var existingEmployeeDb = await _employeeRepository.GetEmployeeById(id);
        if (existingEmployeeDb is null)
        {
            throw new EmployeeIsNotExistingException("Сотрудника с данным id не существует");
        }

        var transaction = await _employeeRepository.BeginTransaction();
        try
        {
            await _employeeRepository.DeleteEmployee(id, transaction);
            await _passportRepository.DeletePassport(id, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }


        await _employeeRepository.DeleteEmployee(id);
    }
}