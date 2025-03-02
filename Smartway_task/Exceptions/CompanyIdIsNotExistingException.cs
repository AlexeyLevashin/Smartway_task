namespace Smartway_task.Exceptions;

public class CompanyIdIsNotExistingException(string message) : BadRequestException(message);