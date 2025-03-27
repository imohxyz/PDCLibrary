using Cinema9.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Cinema9.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _config;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        // Create user
        var user = new AppUser 
        { 
            UserName = model.Email, 
            Email = model.Email,
            FullName = model.FullName
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        // Add role to user
        if (!string.IsNullOrWhiteSpace(model.Role))
        {   
            // Assign role to user
            await _userManager.AddToRoleAsync(user, model.Role);
        }
        else
        {
            // Assign default role if none specified
            await _userManager.AddToRoleAsync(user, "User");
        }
        
        return Ok(new { UserId = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return Unauthorized();
        
        var result = await _signInManager.PasswordSignInAsync(
            model.Email, 
            model.Password, 
            isPersistent: false, 
            lockoutOnFailure: false);
        
        if (!result.Succeeded)
            return Unauthorized();
        
        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);

        return Ok(new { Token = token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    private string GenerateJwtToken(AppUser user, IList<string> roles)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        
        var credentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("fullName", user.FullName)
        };
        
        // Add roles to claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_config["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record RegisterDto {
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string FullName { get; set; } = String.Empty;
    public string? Role { get; set; }

}

public record LoginDto(string Email, string Password);
