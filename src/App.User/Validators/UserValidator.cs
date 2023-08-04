using App.Security;
using App.User.Dtos;
using App.User.Exceptions;
using App.User.Providers.Interfaces;
using App.User.Repositories.Interfaces;
using App.User.Validators.Interfaces;

namespace App.User.Validators;

internal class UserValidator : IUserValidator
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProvider _userProvider;

    public UserValidator(
        IUserRepository userRepository,
        IUserProvider userProvider
        )
    {
        _userRepository = userRepository;
        _userProvider = userProvider;
    }

    public async Task<bool> IsUserExistsAsync(string email)
    {
        return await _userRepository.ExistsAsync(x => x.Email == email);
    }

    public async Task<bool> IsUserNotExistsAsync(string email)
    {
        return await _userRepository.NotExistsAsync(x => x.Email == email);
    }

    public async Task<bool> ValidateEmailOrThrowAsync(string email)
    {
        if (await _userRepository.NotExistsAsync(x => x.Email == email))
            throw new UserNotExistsException(email);

        // todo: validate password or elsewhere

        return true;
    }

    public async Task ValidateRegisterAsync(UserDto dto)
    {
        if (await _userRepository.ExistsAsync(x => x.Email == dto.Email))
            throw new EmailAlreadyExistsException(dto.Email);

        if (await _userRepository.ExistsAsync(x => x.Username == dto.Username))
            throw new UsernameAlreadyExistsException(dto.Username);
    }

    public async Task ValidateLoginAsync(UserDto dto)
    {
        if (await _userRepository.NotExistsAsync(x => x.Email == dto.Email))
            throw new UserNotExistsException(dto.Email);

        var hashedPassword = await _userProvider.GetPasswordAsync(dto.Email);

        var verified = Password.EnhancedVerify(dto.Password, hashedPassword);

        if (!verified)
            throw new InvalidPasswordException();
    }
}
