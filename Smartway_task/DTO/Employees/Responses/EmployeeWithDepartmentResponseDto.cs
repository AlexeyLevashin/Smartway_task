namespace Smartway_task.DTO;

public class EmployeeWithDepartmentResponseDto
{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public PassportResponseDto Passport { get; set; }
        public DepartmentResponseDto Department { get; set; }
}