namespace Smartway_task.Exceptions;

public class DepartmentIsExistingException(string message) : BadRequestException(message);