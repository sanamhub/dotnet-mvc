using App.Base.Uow.Interfaces;
using App.Security;
using App.User.Dtos;
using App.User.Services.Interfaces;
using App.User.Validators.Interfaces;

namespace App.User.Services;

internal class UserService : IUserService
{
    private readonly IUow _uow;
    private readonly IUserValidator _userValidator;

    public UserService(
        IUow uow,
        IUserValidator userValidator
        )
    {
        _uow = uow;
        _userValidator = userValidator;
    }

    public async Task RegisterAsync(UserDto dto)
    {
        await _userValidator.ValidateRegisterAsync(dto);

        var user = new Entities.User
        {
            Username = dto.Username,
            Email = dto.Email,
            Password = Password.EnhancedHash(dto.Password),
            CreatedBy = 0,
        };

        await _uow.AddAsync(user);

        await _uow.SaveChangesAsync();
    }

    public async Task LoginAsync(UserDto dto)
    {
        await _userValidator.ValidateLoginAsync(dto);

        // do stuff
    }
}
