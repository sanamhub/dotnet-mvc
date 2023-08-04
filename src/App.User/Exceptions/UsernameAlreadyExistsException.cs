using App.Base.Exceptions;

namespace App.User.Exceptions;

public class UsernameAlreadyExistsException : BaseException
{
    public UsernameAlreadyExistsException(string username, string? msg = null) : base(msg ?? $"Username {username} already exists") { }
}
