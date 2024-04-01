using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MessagingApp.Api;

public static class ServiceConfiguration
{
    public const string LocalhostCorsPolicy = "AllowLocalhost3000";
    
    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? 
                                           throw new InvalidOperationException("No Jwt Key configured"))),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
            x.MapInboundClaims = false;
        });
        builder.Services.AddAuthorization();
        
        return builder;
    }
    
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(LocalhostCorsPolicy, 
                builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );
        });

        return services;
    }
}