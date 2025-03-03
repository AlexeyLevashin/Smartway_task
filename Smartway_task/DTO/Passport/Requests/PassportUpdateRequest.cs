using System.ComponentModel.DataAnnotations;

namespace Smartway_task.DTO.Passport.Requests;

public class PassportUpdateRequest
{
    [MinLength(1, ErrorMessage = "Тип паспорта не может быть пустым.")]
    public string? Type { get; set; }
    
    [MinLength(1, ErrorMessage = "Номер паспорта не может быть пустым.")]
    public string? Number { get; set; }
}