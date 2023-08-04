using App.Base.Exceptions;

namespace App.User.Exceptions;

public class EmailAlreadyExistsException : BaseException
{
    public EmailAlreadyExistsException(string email, string? msg = null) : base(msg ?? $"User with email {email} already exists") { }
}
