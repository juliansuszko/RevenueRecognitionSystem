using System.Text;
using Infrastructure.Authentication;
using Infrastructure.Authentication.JWT;
using Infrastructure.Authentication.Password;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("Infrastructure")));

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IAccessTokenService, JwtAccessTokenService>();
        
        return services;
    }
    
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWT");
        services.Configure<JwtOptions>(jwtSettings);
        var jwtOptions = jwtSettings.Get<JwtOptions>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SymmetricSecurityKey))
                };
            });
        
        return services;
    }
}