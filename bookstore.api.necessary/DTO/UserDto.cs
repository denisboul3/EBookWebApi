using System.ComponentModel.DataAnnotations;

namespace bookstore.api.DTO;

public class UserDto
{
    [Required(ErrorMessage = "Login is required")]
    public string? Login { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
}

public class CreateUserDto : UserDto { }