namespace Smartway_task.Exceptions;

public class PassportNumberIsExistingException(string message) : BadRequestException(message);