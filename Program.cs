using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Email;
using Whizsheet.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
var smtpUser = builder.Configuration["Email:Smtp:Username"];
var smtpPass = builder.Configuration["Email:Smtp:Password"];

// Controllers
builder.Services.AddControllers();

// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AngularDev", policy =>
	{
		policy
			.WithOrigins("http://localhost:4200")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});


builder.Services.AddDbContext<WhizsheetDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("WhizsheetDb"));
});

builder.Services
	.AddIdentity<ApplicationUser, IdentityRole>(options =>
	{
		options.SignIn.RequireConfirmedEmail = true;

		options.Password.RequiredLength = 8;
		options.Password.RequireDigit = true;
		options.Password.RequireUppercase = true;
		options.Password.RequireLowercase = true;
		options.Password.RequireNonAlphanumeric = false;
	})
	.AddEntityFrameworkStores<WhizsheetDbContext>()
	.AddDefaultTokenProviders();

// =========================
// AUTHENTICATION - JWT
// =========================

builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme =
			JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme =
			JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		var jwtSettings = builder.Configuration.GetSection("Jwt");

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,

			ValidIssuer = jwtSettings["Issuer"],
			ValidAudience = jwtSettings["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
			)
		};
	})
	.AddGoogle(options =>
	{
		options.ClientId =
			builder.Configuration["Authentication:Google:ClientId"]!;
		options.ClientSecret =
			builder.Configuration["Authentication:Google:ClientSecret"]!;
	});

builder.Services.Configure<SmtpSettings>(
	builder.Configuration.GetSection("Email:Smtp"));


var smtpTest = builder.Configuration
	.GetSection("Email:Smtp")
	.Get<SmtpSettings>();
Console.WriteLine("SMTP CONFIG CHECK:");
Console.WriteLine($"Host: {smtpTest?.Host}");
Console.WriteLine($"Port: {smtpTest?.Port}");
Console.WriteLine($"Username: {smtpTest?.Username}");
Console.WriteLine($"From: {smtpTest?.From}");

Console.WriteLine("DB CONNECTION STRING CHECK:");
Console.WriteLine(builder.Configuration.GetConnectionString("WhizsheetDb"));


builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors("AngularDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
