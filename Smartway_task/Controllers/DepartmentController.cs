using Microsoft.AspNetCore.Mvc;
using Smartway_task.DTO;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Controllers;

[Route("api/departments")]
public class DepartmentController: ControllerBase
{
    public IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpPost("")]
    public async Task<IActionResult> AddDepartment(AddNewDepartmentRequestDto addNewDepartmentRequestDto)
    {
        var res = await _departmentService.AddDepartment(addNewDepartmentRequestDto);
        return Ok(res);
    }
    
}