using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Dtos.Auth;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly IConfiguration _configuration;

	public AuthController(
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		IConfiguration configuration)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_configuration = configuration;
	}

	// =========================
	// REGISTER
	// =========================

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterRequest request)
	{
		var user = new ApplicationUser
		{
			UserName = request.Email,
			Email = request.Email
		};

		var result = await _userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
		{
			return BadRequest(result.Errors);
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

		var confirmationLink =
			$"http://localhost:4200/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

		// TODO: envoyer l’email via IEmailSender
		Console.WriteLine("CONFIRM EMAIL LINK:");
		Console.WriteLine(confirmationLink);

		return Ok("Registration successful. Check your email.");
	}

	// =========================
	// LOGIN
	// =========================

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user == null)
		{
			return Unauthorized("Invalid credentials");
		}

		if (!user.EmailConfirmed)
		{
			return Unauthorized("Email not confirmed");
		}

		var result = await _signInManager.CheckPasswordSignInAsync(
			user,
			request.Password,
			false
		);

		if (!result.Succeeded)
		{
			return Unauthorized("Invalid credentials");
		}

		var token = GenerateJwt(user);

		return Ok(new AuthResponse
		{
			Token = token
		});
	}

	// =========================
	// CONFIRM EMAIL
	// =========================

	[HttpGet("confirm-email")]
	public async Task<IActionResult> ConfirmEmail(
		[FromQuery] string userId,
		[FromQuery] string token)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user == null)
		{
			return BadRequest("Invalid user");
		}

		var result = await _userManager.ConfirmEmailAsync(user, token);

		if (!result.Succeeded)
		{
			return BadRequest("Invalid token");
		}

		return Ok("Email confirmed successfully");
	}

	// =========================
	// JWT GENERATION
	// =========================

	private string GenerateJwt(ApplicationUser user)
	{
		var jwtSettings = _configuration.GetSection("Jwt");

		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
			new Claim(JwtRegisteredClaimNames.Email, user.Email!)
		};

		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
		);

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: jwtSettings["Issuer"],
			audience: jwtSettings["Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(
				int.Parse(jwtSettings["ExpiresMinutes"]!)
			),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
