using FluentAssertions.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Key")),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", // Asegura que reconoce el claim de rol
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
        };
    });

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminPolicy", policy =>
            policy.RequireRole("Administrador"));
    });


builder.Services.AddAuthorization();


var app = builder.Build();


app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 403)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\": \"Debe iniciar sesión como administrador para realizar esta acción.\"}");
    }
    else if (context.Response.StatusCode == 401)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\": \"Debe iniciar sesión para acceder a esta acción.\"}");
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
