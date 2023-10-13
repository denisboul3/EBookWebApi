using System.ComponentModel.DataAnnotations;

namespace bookstore.api.DTO;

public class LoginDto
{
    [Required]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }
}

public class AuthDto
{
    public string? Token { get; set; }
}

public class ChangePasswordDto
{
    [Required]
    public string? OldPassword { get; set; }

    [Required]
    public string? NewPassword { get; set; }
}