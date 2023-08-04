using App.Base.Exceptions;

namespace App.User.Exceptions;

internal class InvalidPasswordException : BaseException
{
    public InvalidPasswordException() : base("Invalid password.")
    {
    }
}
