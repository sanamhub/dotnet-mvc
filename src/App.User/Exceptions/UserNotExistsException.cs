using App.Base.Exceptions;

namespace App.User.Exceptions;

public class UserNotExistsException : BaseException
{
    public UserNotExistsException(string email, string? msg = null) : base(msg ?? $"User with email {email} not exists") { }
}
