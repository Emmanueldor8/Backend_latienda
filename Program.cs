using LaTiendaApi.Mappings;
using LaTiendaApi.Models;
using LaTiendaApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Servicios

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<LatiendaContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("cadenaSQL"),
        sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercelAndLocal", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion

var app = builder.Build();

#region Seed inicial

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<LatiendaContext>();

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Nombre = "Admin" },
                new Role { Nombre = "User" }
            );
        }

        if (!context.Categorias.Any())
        {
            context.Categorias.Add(new Categoria
            {
                Nombre = "General",
                Estado = true
            });
        }

        context.SaveChanges();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos.");
    }
}

#endregion

#region Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LaTienda API");
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowVercelAndLocal");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();