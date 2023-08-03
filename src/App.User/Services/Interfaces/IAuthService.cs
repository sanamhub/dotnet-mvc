namespace App.User.Services.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
}
