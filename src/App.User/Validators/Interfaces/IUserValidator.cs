namespace App.User.Validators.Interfaces;

internal interface IUserValidator
{
    Task<bool> IsUserExistsAsync(string email);

    Task<bool> ValidateEmailOrThrowAsync(string email);
}
