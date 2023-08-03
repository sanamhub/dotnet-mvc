using App.User.Repositories.Interfaces;
using App.User.Services.Interfaces;
using App.User.Validators.Interfaces;

namespace App.User.Services;

internal class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _userValidator;

    public AuthService(
        IUserRepository userRepository,
        IUserValidator userValidator
        )
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        await _userValidator.ValidateEmailOrThrowAsync(email);



        throw new NotImplementedException();
    }
}
