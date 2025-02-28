namespace Smartway_task.Models;

public class DbEmployee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int CompanyId { get; set; }
    public DbPassport DbPassport { get; set; }
    public DbDepartment DbDepartment { get; set; }
    
    public int DepartmentId { get; set; }
}