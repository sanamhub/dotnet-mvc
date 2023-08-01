using System.ComponentModel.DataAnnotations;

namespace App.Base.Entities;

public class BaseEntity
{
    [Key]
    public long Id { get; set; }
}
