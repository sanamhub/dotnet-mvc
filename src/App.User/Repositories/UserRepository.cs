using App.Base.Repositories;
using App.User.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.User.Repositories;

internal class UserRepository : Repository<Entities.User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }
}
