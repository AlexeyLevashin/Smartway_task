using Smartway_task.DTO;
using Smartway_task.Exceptions;
using Smartway_task.Mappers;
using Smartway_task.Models;
using Smartway_task.NewDto.Employee.Requests;
using Smartway_task.NewDto.Employee.Responses;
using Smartway_task.Repositories.Interfaces;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Services;

public class EmployeeService:IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    
    public async Task<EmployeeResponse> AddEmployee(AddNewEmployeeRequest addNewEmployeeRequest)
    {
        if (await _employeeRepository.GetEmployeeByPhone(addNewEmployeeRequest.Phone) is not null)
        {
            throw new PhoneIsExistingException("Сотрудник с данным номером телефона уже существует");
        }
        
        DbEmployee dbEmployee = addNewEmployeeRequest.MapToDb();
        
        dbEmployee.DbPassport = addNewEmployeeRequest.NewPassport.MapToDb();

        DbEmployee addedDbEmployee = await _employeeRepository.AddEmployee(dbEmployee);
        
        EmployeeResponse employeeResponse = addedDbEmployee.MapToDto();

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

    // public async Task<DbEmployee> UpdateEmployee(EmployeeUpdateRequestDto employeeUpdateRequestDto, int id)
    // {
    //     var existingEmployeeDb = await _employeeRepository.GetEmployeeById(id);
    //     if (existingEmployeeDb is null)
    //     {
    //         throw new EmployeeIsNotExistingException("Сотрудник с данным id не существует");
    //     }
    //     
    //
    //     var existingEmployeeDomain = existingEmployeeDb.MapToDomainUpd();
    //
    //
    //     var updatedEmployeeDb = existingEmployeeDomain.ApplyChangesFromDto(employeeUpdateRequestDto);
    //     updatedEmployeeDb = await _employeeRepository.UpdateEmployee(updatedEmployeeDb, id);
    //
    //     return updatedEmployeeDb;
    //     
    //
    // }
    
    
    public async Task<DbEmployee> UpdateEmployee(EmployeeUpdateRequest employeeUpdateRequest, int id)
    {
        var existingEmployeeDb = await _employeeRepository.GetEmployeeById(id);
        if (existingEmployeeDb is null)
        {
            throw new EmployeeIsNotExistingException("Сотрудник с данным id не существует");
        }
        
        
        var dbEmployee = existingEmployeeDb.ApplyChangesFromDto(employeeUpdateRequest);

        
        _employeeRepository.UpdateEmployee(existingEmployeeDb, id); 
        

        return existingEmployeeDb;
    }

    public async Task DeleteEmployee(int id)
    {
        var existingEmployeeDb = await _employeeRepository.GetEmployeeById(id);
        if (existingEmployeeDb is null)
        {
            throw new EmployeeIsNotExistingException("Сотрудника с данным id не существует");
        }

        await _employeeRepository.DeleteEmployee(id);
    }





}
    