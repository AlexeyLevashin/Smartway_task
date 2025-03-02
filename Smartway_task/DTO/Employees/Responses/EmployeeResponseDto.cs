using Smartway_task.Models;

namespace Smartway_task.DTO;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int CompanyId { get; set; }
    public int DepartmentId { get; set; }
    public PassportResponseDto Passport { get; set; }
}