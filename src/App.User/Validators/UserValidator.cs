using App.User.Exceptions;
using App.User.Repositories.Interfaces;
using App.User.Validators.Interfaces;

namespace App.User.Validators;

internal class UserValidator : IUserValidator
{
    private readonly IUserRepository _userRepository;

    public UserValidator(
        IUserRepository userRepository
        )
    {
        _userRepository = userRepository;
    }

    public async Task<bool> IsUserExistsAsync(string email)
    {
        return await _userRepository.ExistsAsync(x => x.Email == email);
    }

    public async Task<bool> ValidateEmailOrThrowAsync(string email)
    {
        if (await _userRepository.NoExistsAsync(x => x.Email == email))
            throw new UserNotExistsException(email);

        return true;
    }
}
