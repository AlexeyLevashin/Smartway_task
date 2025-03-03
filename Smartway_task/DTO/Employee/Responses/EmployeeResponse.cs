using Smartway_task.DTO.Passport.Responses;

namespace Smartway_task.DTO.Employee.Responses;

public class EmployeeResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int CompanyId { get; set; }
    public int DepartmentId { get; set; }
    public PassportResponse Passport { get; set; }
}