namespace Smartway_task.Models;

public class DbPassport
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Number { get; set; }
    public int EmployeeId { get; set; }
}