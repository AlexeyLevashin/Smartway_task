using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Smartway_task.DTO;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Controllers;

[Route("api/emloyees")]
public class EmployeeController: ControllerBase
{
    public IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost("")]
    public async Task<IActionResult> AddEmployee(AddNewEmployeeRequestDto addNewEmployeeRequestDto)
    {
        var res = await _employeeService.AddEmployee(addNewEmployeeRequestDto);
        return Ok(res.Id);
    }

    [HttpGet("byCompanyId")]
    public async Task<IActionResult> GetEmployeesByCompanyId(EmployeeResponseDto employeeResponseDto)
    {
        var res = await _employeeService.GetEmployeesByCompanyId(employeeResponseDto.CompanyId);
        return Ok(res);
    }

    [HttpGet("byDepartmentId")]
    public async Task<IActionResult> GetEmplooyeesByDepartmentId(EmployeeResponseDto employeeResponseDto)
    {
        var res = await _employeeService.GetEmployeesByDepartmentId(employeeResponseDto.DepartmentId);
        return Ok(res);
    }
    
    
    
    
    
}