using core.Exceptions.Base;

namespace core.Exceptions.User;

public class UserNotFoundHttpException : NotFoundHttpExceptionBase
{
    public UserNotFoundHttpException(Guid userId) : base($"User with the id \"{userId}\" not found")
    {
    }

    public UserNotFoundHttpException(string email) : base($"User with the email \"{email}\" not found")
    {
    }
}