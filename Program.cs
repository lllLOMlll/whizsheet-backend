using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Whizsheet.Api.Domain;
using Whizsheet.Api.Infrastructure;

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


var app = builder.Build(); builder.Services.AddAuthentication()
	.AddGoogle(options =>
	{
		options.ClientId =
			builder.Configuration["Authentication:Google:ClientId"]!;
		options.ClientSecret =
			builder.Configuration["Authentication:Google:ClientSecret"]!;
	});



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
