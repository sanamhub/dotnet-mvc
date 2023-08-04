using App.User.Providers.Interfaces;
using App.User.Repositories.Interfaces;

namespace App.User.Providers;

internal class UserProvider : IUserProvider
{
    private readonly IUserRepository _userRepository;

    public UserProvider(
        IUserRepository userRepository
        )
    {
        _userRepository = userRepository;
    }

    public async Task<string> GetPasswordAsync(string email)
    {
        return (await _userRepository.GetSingleAsync(x => x.Email == email)).Email;
    }
}
