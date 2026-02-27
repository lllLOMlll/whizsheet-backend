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

	private string FrontendBaseUrl =>
	_configuration["Frontend:BaseUrl"]!.TrimEnd('/');


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
	public async Task<IActionResult> Register([FromBody] RegisterRequest request)
	{
		Console.WriteLine("===== REGISTER START =====");

		// 1. Protection contre les doublons (évite l'erreur 500 si Angular appelle deux fois)
		var existingUser = await _userManager.FindByEmailAsync(request.Email);
		if (existingUser != null)
		{
			// Si l'utilisateur existe déjà mais n'est pas confirmé, on pourrait techniquement 
			// renvoyer un succès pour ne pas donner d'info aux hackers, ou un BadRequest 
			// si tu préfères que l'utilisateur sache pourquoi ça bloque.
			return BadRequest(new[] { new { description = "Email is already registered." } });
		}

		ApplicationUser user = new ApplicationUser
		{
			UserName = request.Email,
			Email = request.Email
		};

		// 2. Création de l'utilisateur
		var createResult = await _userManager.CreateAsync(user, request.Password);

		if (!createResult.Succeeded)
		{
			return BadRequest(createResult.Errors);
		}

		try
		{
			// 3. Génération du token et encodage URI (le plus fiable pour Azure)
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var encodedToken = Uri.EscapeDataString(token);

			var confirmationLink = $"{FrontendBaseUrl}/confirm-email?userId={user.Id}&token={encodedToken}";

			// 4. Envoi de l'email (Ton message original conservé intégralement)
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

			Console.WriteLine("REGISTER SUCCESS AND EMAIL SENT");
			return Ok(new { message = "Registration successful. Check your email." });
		}
		catch (Exception ex)
		{
			// Log de l'erreur pour diagnostic dans Azure Logs
			Console.WriteLine($"ERROR DURING EMAIL SENDING: {ex.Message}");

			// On retourne un 500 car l'utilisateur est créé mais le service de mail a flanché
			return StatusCode(500, new { message = "User created but failed to send confirmation email." });
		}
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
	[FromQuery] string token) // Le token est automatiquement décodé de l'URL par ASP.NET
	{
		var user = await _userManager.FindByIdAsync(userId);

		// 1. Utilisateur inexistant
		if (user == null)
			return Ok(new { confirmed = false });

		// 2. Si déjà confirmé (gère les doubles clics/scanners d'emails)
		if (user.EmailConfirmed)
			return Ok(new { confirmed = true });

		try
		{
			// On utilise le token directement, sans redécodage manuel
			var result = await _userManager.ConfirmEmailAsync(user, token);

			if (result.Succeeded)
				return Ok(new { confirmed = true });

			// 3. Double vérification de sécurité au cas où une requête concurrente
			// aurait réussi il y a quelques millisecondes.
			var updatedUser = await _userManager.FindByIdAsync(userId);
			return Ok(new { confirmed = updatedUser?.EmailConfirmed ?? false });
		}
		catch
		{
			// On vérifie une dernière fois l'état en DB avant d'abandonner
			var finalUser = await _userManager.FindByIdAsync(userId);
			return Ok(new { confirmed = finalUser?.EmailConfirmed ?? false });
		}
	}


	// =========================
	// JWT GENERATION
	// =========================

	private string GenerateJwt(ApplicationUser user)
	{
		var jwtSettings = _configuration.GetSection("Jwt");

	var claims = new List<Claim>
	{
		// Claim principal attendu par ASP.NET Identity
		new Claim(ClaimTypes.NameIdentifier, user.Id),

		// Claim standard JWT (interop, OAuth, OpenID)
		new Claim(JwtRegisteredClaimNames.Sub, user.Id),

		// Email (utile côté frontend et logs)
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
	public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user == null || user.EmailConfirmed)
		{
			return Ok(); // On ne révèle pas l'existence du compte
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		var encodedToken = Uri.EscapeDataString(token);

		var confirmationLink = $"{FrontendBaseUrl}/confirm-email?userId={user.Id}&token={encodedToken}";

		await _emailSender.SendAsync(
			user.Email!,
			"Confirm your Whizsheet account",
			$"""
        <p>You requested a new confirmation email.</p>
        <p><a href="{confirmationLink}">Confirm email</a></p>
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

		var frontendUrl =
			$"{FrontendBaseUrl}/auth-redirect?token={token}";

		return Redirect(frontendUrl);

	}




}
