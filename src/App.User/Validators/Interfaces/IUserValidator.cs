using App.User.Dtos;

namespace App.User.Validators.Interfaces;

public interface IUserValidator
{
    Task<bool> IsUserExistsAsync(string email);

    Task<bool> IsUserNotExistsAsync(string email);

    Task<bool> ValidateEmailOrThrowAsync(string email);

    Task ValidateRegisterAsync(UserDto dto);

    Task ValidateLoginAsync(UserDto dto);
}
