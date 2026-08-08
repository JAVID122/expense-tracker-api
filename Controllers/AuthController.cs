using Microsoft.AspNetCore.Authorization;
using Budget.Api.Data;
using Budget.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Budget.Api.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _config;

		public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config)
		{
			_userManager = userManager;
			_config = config;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterDto dto)
		{
			var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
			var result = await _userManager.CreateAsync(user, dto.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok(new { message = "User registered successfully" });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null)
				return Unauthorized(new { message = "Invalid credentials" });

			var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
			if (!validPassword)
				return Unauthorized(new { message = "Invalid credentials" });

			var token = GenerateJwtToken(user);
			return Ok(new { token });
		}
		[Authorize]
		[HttpGet("me")]
		public IActionResult Me()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
						 ?? User.FindFirst("sub")?.Value;
			return Ok(new { userId });
		}

		private string GenerateJwtToken(ApplicationUser user)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]!)),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}