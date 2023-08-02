using System.ComponentModel.DataAnnotations.Schema;
using App.Base.Constants;
using App.Base.Entities;

namespace App.User.Entities;

[Table("Users", Schema = Schema.User)]
internal class User : BaseAuditableEntity
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string PasswordSalt { get; set; } = default!;
}
