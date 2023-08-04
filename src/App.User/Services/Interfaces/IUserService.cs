using App.User.Dtos;

namespace App.User.Services.Interfaces;

public interface IUserService
{
    Task RegisterAsync(UserDto dto);

    Task LoginAsync(UserDto dto);
}
