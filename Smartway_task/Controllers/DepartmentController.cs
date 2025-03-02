using Microsoft.AspNetCore.Mvc;
using Smartway_task.NewDto.Department.Requests;
using Smartway_task.Services.Interfaces;

namespace Smartway_task.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpPost("")]
    public async Task<IActionResult> AddDepartment(DepartmentRequest departmentRequest)
    {
        var res = await _departmentService.AddDepartment(departmentRequest);
        return Ok(res);
    }
}