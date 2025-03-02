using System.ComponentModel.DataAnnotations;

namespace Smartway_task.DTO;

public class AddNewDepartmentRequestDto
{
    [Required(ErrorMessage = "Название отдела обязательно для заполнения.")]
    [MinLength(1, ErrorMessage = "Название отдела не может быть пустым.")]
    public string Name { get; set; }

    [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Номер телефона должен содержать только цифры и может начинаться с '+', длина от 10 до 15 символов.")]
    public string Phone { get; set; }
}