using System.ComponentModel.DataAnnotations;
using Smartway_task.NewDto.Passport.Requests;

namespace Smartway_task.NewDto.Employee.Requests;

public class AddNewEmployeeRequest
{
    [Required(ErrorMessage = "Имя обязательно для заполнения.")]
    [MinLength(1, ErrorMessage = "Имя не может быть пустым.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна для заполнения.")]
    [MinLength(1, ErrorMessage = "Фамилия не может быть пустой.")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Телефон обязателен для заполнения.")]
    [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Телефон должен содержать от 10 до 15 цифр и может начинаться с '+'.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "CompanyId обязателен для заполнения.")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "DepartmentId обязателен для заполнения.")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Паспорт обязателен для заполнения.")]
    public NewPassportRequest NewPassport { get; set; }
}