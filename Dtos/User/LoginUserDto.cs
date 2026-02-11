using System.ComponentModel.DataAnnotations;

namespace PostHubAPI.Dtos.User;

public class LoginUserDto
{
    [Required]
    [StringLength(20)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}