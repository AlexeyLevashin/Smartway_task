using Smartway_task.Domain;
using Smartway_task.Models;

namespace Smartway_task.DTO;

public class AddNewEmployeeRequestDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int CompanyId { get; set; }
    public int DepartmentId { get; set; }
    public AddNewPassportRequestDto NewPassport { get; set; }
    public AddDepartmentRequestDto NewDepartment { get; set; }
}