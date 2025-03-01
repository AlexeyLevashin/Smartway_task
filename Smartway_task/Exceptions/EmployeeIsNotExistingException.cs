namespace Smartway_task.Exceptions;

public class EmployeeIsNotExistingException(string message) : BadRequestException(message);