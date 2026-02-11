using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PostHubAPI.Dtos.User;
using PostHubAPI.Models;
using PostHubAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PostHubAPI.Services.Implementations;

public class UserService(IConfiguration configuration, UserManager<User> userManager)
    : IUserService
{
    public async Task<string> Register(RegisterUserDto dto)
    {
        User? userByEmail = await userManager.FindByEmailAsync(dto.Email);
        if (userByEmail != null)
        {
            throw new ArgumentException($"User with {dto.Email} already exists!!");
        }

        User user = new User()
        {
            Email = dto.Email,
            UserName = dto.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        IdentityResult result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            throw new ArgumentException(
                $"Unable to register user {dto.Username} errors: {GetErrorsText(result.Errors)}");
        }

        return await Login(new LoginUserDto { Username = dto.Username, Password = dto.Password });
    }

    public async Task<string> Login(LoginUserDto dto)
    {
        User? user = await userManager.FindByNameAsync(dto.Username);
        if (user == null)
        {
            throw new ArgumentException($"Name {dto.Username} is not registered!");
        }

        if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new ArgumentException($"Unable to authenticate user {dto.Username}!");
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        JwtSecurityToken token = GetToken(claims);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> claims)
    {
        SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

        JwtSecurityToken token = new JwtSecurityToken
        (
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private static string GetErrorsText(IEnumerable<IdentityError> errors)
    {
        return string.Join(", ", errors.Select(error => error.Description).ToArray());
    }
}