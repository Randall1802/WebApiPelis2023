using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using WebApiPelis2023;

var builder = WebApplication.CreateBuilder(args);
var MyCors = "MyCors";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//conxión a la bd de postgres, el defaultConnection se crea en appsettings
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//para sql: (somee)
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//config seguridad de identity (microsoft) para usar jwt. tablas identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//para funcionalidades de automapper (22-11-2023)
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//usar cors
builder.Services.AddCors(options => options.AddPolicy(MyCors, builder => builder.AllowAnyHeader().
                                                    AllowAnyMethod().AllowAnyOrigin()));

//config JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones => 
opciones.TokenValidationParameters = new TokenValidationParameters
{ //parametros q va a tener o no el jwt. investigarlos
	ValidateIssuer=false,
	ValidateAudience=false,
	ValidateLifetime=true,
	ValidateIssuerSigningKey=true,
	IssuerSigningKey = new SymmetricSecurityKey(
		Encoding.UTF8.GetBytes(builder.Configuration["LlaveJWT"]!)),
	ClockSkew=TimeSpan.Zero
});

//config swagger para usar auth de windows pa usar los jwt
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme, 
					Id = "Bearer"
				}
			},
			new string[]{}
		}
	});
});


//todas las config van antes de aquí. porque usa todo lo de arriba
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
