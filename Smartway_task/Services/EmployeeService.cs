using Smartway_task.DTO;
using Smartway_task.Exceptions;
using Smartway_task.Mappers;
using Smartway_task.Models;
using Smartway_task.Repositories.Interfaces;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Services;

public class EmployeeService:IEmployeeService
{
    public IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeResponseDto> AddEmployee(AddNewEmployeeRequestDto addNewEmployeeRequestDto)
    {
        if (await _employeeRepository.GetEmployeeByPhone(addNewEmployeeRequestDto.Phone) is not null)
        {
            throw new PhoneIsExistingException("Сотрудник с данным номером телефона уже существует");
        }
        
        return(await _employeeRepository.AddEmployee(
            new DbEmployee
            {
                Name = addNewEmployeeRequestDto.Name,
                Surname = addNewEmployeeRequestDto.Surname,
                Phone = addNewEmployeeRequestDto.Phone,
                CompanyId = addNewEmployeeRequestDto.CompanyId,
                DepartmentId = addNewEmployeeRequestDto.DepartmentId,
                DbPassport = addNewEmployeeRequestDto.NewPassport.MapToDomain().MapToDb(),
                DbDepartment = addNewEmployeeRequestDto.NewDepartment.MapToDomain().MapToDb()
            })).MapToDomain().MapToDto();
    }

    public async Task<List<EmployeeResponseDto>> GetEmployeesByCompanyId(int companyId)
    {
        return ( await _employeeRepository.GetEmployeeByCompanyId(companyId)).MapToDomain().MapToDto();
    }

    public async Task<List<EmployeeResponseDto>> GetEmployeesByDepartmentId(int departmentId)
    {
        return (await _employeeRepository.GetEmployeeByDepartmentId(departmentId)).MapToDomain().MapToDto();
    }
        
    
        
        
        
        
        
        
        
        
        
        
        
        
        
        
}