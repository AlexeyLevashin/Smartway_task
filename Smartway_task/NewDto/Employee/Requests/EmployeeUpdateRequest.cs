using System.ComponentModel.DataAnnotations;
using Smartway_task.NewDto.Passport.Requests;

namespace Smartway_task.NewDto.Employee.Requests;

public class EmployeeUpdateRequest
{
    [MinLength(1, ErrorMessage = "Имя не может быть пустым.")]
    public string? Name { get; set; }

    [MinLength(1, ErrorMessage = "Фамилия не может быть пустой.")]
    public string? Surname { get; set; }

    [RegularExpression(@"^\+?[0-9]{10,15}$",
        ErrorMessage = "Телефон должен содержать от 10 до 15 цифр и может начинаться с '+'.")]
    public string? Phone { get; set; }

    public int? CompanyId { get; set; }

    public int? DepartmentId { get; set; }

    public PassportUpdateRequest? Passport { get; set; }
}