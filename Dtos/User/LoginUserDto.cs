using System.ComponentModel.DataAnnotations;

namespace PostHubAPI.Dtos.User;

public class LoginUserDto
{
    [Required]
    [StringLength(20)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}