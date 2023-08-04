namespace App.User.Providers.Interfaces;

public interface IUserProvider
{
    Task<string> GetPasswordAsync(string email);
}
