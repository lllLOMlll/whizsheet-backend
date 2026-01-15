using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AngularDev");

app.MapControllers();

app.Run();
