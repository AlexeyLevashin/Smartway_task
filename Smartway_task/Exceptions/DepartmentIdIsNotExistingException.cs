namespace Smartway_task.Exceptions;

public class DepartmentIdIsNotExistingException(string message) : BadRequestException(message);