using Microsoft.AspNetCore.Mvc;
using Smartway_task.NewDto.Employee.Requests;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Controllers;

[ApiController]
[Route("api")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost("/employees")]
    public async Task<IActionResult> AddEmployee(AddNewEmployeeRequest addNewEmployeeRequest)
    {
        var res = await _employeeService.AddEmployee(addNewEmployeeRequest);
        return Ok(res.Id);
    }

    [HttpGet("/companies/{id}/employees")]
    public async Task<IActionResult> GetEmployeesByCompanyId(int id)
    {
        var res = await _employeeService.GetEmployeesByCompanyId(id);
        return Ok(res);
    }

    [HttpGet("/departments/{id}/employees")]
    public async Task<IActionResult> GetEmplooyeesByDepartmentId(int id)
    {
        var res = await _employeeService.GetEmployeesByDepartmentId(id);
        return Ok(res);
    }

    [HttpPut("/employees/{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateRequest employeeUpdateRequestDto)
    {
        var res = await _employeeService.UpdateEmployee(employeeUpdateRequestDto, id);
        return Ok(res);
    }

    [HttpDelete("/employees/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await _employeeService.DeleteEmployee(id);
        return Ok();
    }
}