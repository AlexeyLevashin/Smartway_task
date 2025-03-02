using Smartway_task.NewDto.Department.Responses;
using Smartway_task.NewDto.Passport.Responses;

namespace Smartway_task.NewDto.Employee.Responses;

public class EmployeeWithDepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int CompanyId { get; set; }
    public int DepartmentId { get; set; }
    public PassportResponse Passport { get; set; }
    public DepartmentResponse Department { get; set; }
}