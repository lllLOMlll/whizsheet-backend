using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AngularDev");

app.MapControllers();

app.Run();
