using core.Exceptions.Base;

namespace core.Exceptions.User;

public class RevokeUserRefreshTokenBadRequestHttpException(string message) : BadRequestHttpExceptionBase(message);