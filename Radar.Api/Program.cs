global using Radar.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;
using Radar.Api.Data;
using Swashbuckle.AspNetCore.Filters;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddDbContext<RadarContext>();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(GetAppKey(builder)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
        {
            Description = "JWT authentication (bearer token)",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Name = "authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });
    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    File.WriteAllText("error.txt", ex.ToString());
    throw;
}

static byte[] GetAppKey(WebApplicationBuilder builder)
{
    byte[] rawkey = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    byte[] key = new byte[16];
    Array.Copy(rawkey, key, Math.Min(rawkey.Length, key.Length));
    return key;
}