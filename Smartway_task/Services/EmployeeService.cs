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
                DbPassport = addNewEmployeeRequestDto.NewPassport.MapToDomain().MapToDb()
            })).MapToDomain().MapToDto();
    }

    public async Task<List<EmployeeWithDepartmentResponseDto>> GetEmployeesByCompanyId(int companyId)
    {
        var res = await _employeeRepository.GetEmployeeByCompanyId(companyId);
        return res.MapToDto();
    }

    public async Task<List<EmployeeWithDepartmentResponseDto>> GetEmployeesByDepartmentId(int departmentId)
    {
        var res = await _employeeRepository.GetEmployeeByDepartmentId(departmentId);
        return res.MapToDto();
    }

    public async Task<DbEmployee> UpdateEmployee(EmployeeUpdateRequestDto employeeUpdateRequestDto)
    {
        var existingEmployeeDb = await _employeeRepository.GetEmployeeById(employeeUpdateRequestDto.Id);
        if (existingEmployeeDb is null)
        {
            throw new EmployeeIsNotExistingException("Сотрудника с данным id не существует");
        }
        

        var existingEmployeeDomain = existingEmployeeDb.MapToDomainUpd();


        var updatedEmployeeDb = existingEmployeeDomain.ApplyChangesFromDto(employeeUpdateRequestDto);
        updatedEmployeeDb = await _employeeRepository.UpdateEmployee(updatedEmployeeDb);

        return updatedEmployeeDb;
        

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
    