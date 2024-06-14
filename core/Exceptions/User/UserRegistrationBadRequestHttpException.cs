using core.Exceptions.Base;

namespace core.Exceptions.User;

public class UserRegistrationBadRequestHttpException(string message) : BadRequestHttpExceptionBase(message);