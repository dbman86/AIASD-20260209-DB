using System.ComponentModel.DataAnnotations;

namespace PostHubAPI.Dtos.User;

public class RegisterUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}