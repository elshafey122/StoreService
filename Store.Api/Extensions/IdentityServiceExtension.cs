using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Services.Contract;
using Store.Core.Entities.Identity;
using Store.Repository.Identity;
using Store.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Talabat.APIs.Extensions;

public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped(typeof(IAuthService), typeof(AuthService));

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequiredUniqueChars = 2;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        }).AddEntityFrameworkStores<AppIdentityDbContext>();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]);
                var requiredKeyLength = 256 / 8; // 256 bits/8 = 32 byte    256 bits: This is a common requirement for JWT tokens using the HS256 algorithm we convert to array of bytes[32]
                if (secretKey.Length < requiredKeyLength)
                {
                    // Pad the key to meet the required length
                    Array.Resize(ref secretKey, requiredKeyLength); // resize array of secretkey bytes to be 32 byte by adding 0 in expanding indexing
                }

                // Configure authentication handler
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDays"]))
                };

            });


        return services;
    }




}
