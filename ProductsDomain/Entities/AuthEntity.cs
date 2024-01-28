using System.ComponentModel.DataAnnotations;

namespace ProductsDomain.Entities;

public class AuthEntity
{
    [Key]
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}