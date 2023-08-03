using App.Base.Exceptions;

namespace App.User.Exceptions;

public class UserAlreadyExistsException : BaseException
{
    public UserAlreadyExistsException(string email, string? msg = null) : base(msg ?? $"User with email {email} already exists") { }
}
