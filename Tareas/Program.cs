using Jose;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tareas.Application.Services;
using Tareas.Infrastructure.Context;
using Tareas.Jwt;
using JwtSettings = Tareas.Jwt.JwtSettings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod());
});

builder.Services.AddControllers();

//String connection
var conn = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));
//Servicios
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TareaService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Cargar configuraciones de JwtSettings desde appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


// Configurar la autenticación JWT usando la clave secreta desde la configuración
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Obtener la configuración desde appsettings
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])), // Usar la clave secreta desde la configuración
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"], // Verificar el emisor
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"], // Verificar la audiencia
            ValidateLifetime = true, // Asegurarse de que el token no haya expirado
            ClockSkew = TimeSpan.Zero // No permitir tolerancia de expiración
        };
    });

// Agregar servicios de autorización (si es necesario)
builder.Services.AddAuthorization();
builder.Services.AddSingleton<TokenService>();

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Tareas protegida con JWT",
        Version = "v1"
    });

    // Configuración del Bearer token en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Por favor ingresa el Bearer Token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// Configuración del pipeline de middleware
app.UseAuthentication(); // Habilitar la autenticación
app.UseAuthorization(); // Habilitar la autorización

app.MapControllers();

app.Run();
