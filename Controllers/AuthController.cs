using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Dtos.Auth;
using Whizsheet.Api.Email;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly IConfiguration _configuration;
	private readonly IEmailSender _emailSender;

	public AuthController(
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		IConfiguration configuration,
		IEmailSender emailSender)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_configuration = configuration;
		_emailSender = emailSender;
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

		var encodedToken = WebEncoders.Base64UrlEncode(
			Encoding.UTF8.GetBytes(token)
		);

		var confirmationLink =
		  $"http://localhost:4200/confirm-email" +
		  $"?userId={user.Id}&token={encodedToken}";



		await _emailSender.SendAsync(
			user.Email!,
			"Confirm your Whizsheet account",
			$"""
			<p>Welcome to <strong>Whizsheet</strong> 👋</p>
			<p>Please confirm your email by clicking the link below:</p>
			<p>
				<a href="{confirmationLink}">
					Confirm email
				</a>
			</p>
			"""
		);


		return Ok(new
		{
			message = "Registration successful. Check your email."
		});

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
			return Ok(new { confirmed = false });

		if (user.EmailConfirmed)
			return Ok(new { confirmed = true });

		var decodedToken = Encoding.UTF8.GetString(
			WebEncoders.Base64UrlDecode(token)
		);

		var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

		return Ok(new { confirmed = result.Succeeded });
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

	// =========================
	// RESEND CONFIRMATION EMAIL
	// =========================

	[HttpPost("resend-confirmation")]
	public async Task<IActionResult> ResendConfirmationEmail(
		[FromBody] ResendConfirmationRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		// ⚠️ IMPORTANT : ne jamais révéler si l’email existe
		if (user == null)
		{
			return Ok();
		}

		if (user.EmailConfirmed)
		{
			return Ok();
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		var encodedToken = WebEncoders.Base64UrlEncode(
			Encoding.UTF8.GetBytes(token)
		);

		var confirmationLink =
			$"http://localhost:4200/confirm-email" +
			$"?userId={user.Id}&token={encodedToken}";

		await _emailSender.SendAsync(
				user.Email!,
				"Confirm your Whizsheet account",
				$"""
			<p>You requested a new confirmation email.</p>
			<p>
				<a href="{confirmationLink}">
					Confirm email
				</a>
			</p>
			"""
			);


		return Ok();
	}

	// =========================
	// GOOGLE LOGIN (START)
	// =========================

	[HttpGet("google")]
	public IActionResult GoogleLogin()
	{
		var redirectUrl = Url.Action(
			nameof(GoogleCallback),
			"Auth",
			values: null,
			protocol: Request.Scheme
		);


		var properties = _signInManager
			.ConfigureExternalAuthenticationProperties(
				"Google",
				redirectUrl
			);

		return Challenge(properties, "Google");
	}

	// =========================
	// GOOGLE LOGIN (CALLBACK)
	// =========================

	[HttpGet("google-callback")]
	public async Task<IActionResult> GoogleCallback()
	{
		var info = await _signInManager.GetExternalLoginInfoAsync();

		if (info == null)
		{
			return Unauthorized("Google login failed");
		}

		var email = info.Principal.FindFirstValue(ClaimTypes.Email);

		if (email == null)
		{
			return Unauthorized("Email not provided by Google");
		}

		var user = await _userManager.FindByEmailAsync(email);

		if (user == null)
		{
			user = new ApplicationUser
			{
				UserName = email,
				Email = email,
				EmailConfirmed = true
			};

			var result = await _userManager.CreateAsync(user);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			await _userManager.AddLoginAsync(user, info);
		}

		var token = GenerateJwt(user);

		// ⚠️ Redirection vers Angular avec JWT
		var frontendUrl =
			$"http://localhost:4200/auth-redirect?token={token}";

		return Redirect(frontendUrl);
	}




}
