namespace Smartway_task.Exceptions;

public class PhoneIsExistingException(string message) : BadRequestException(message);