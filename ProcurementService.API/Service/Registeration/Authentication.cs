using ProcurementService.API.Service.Registeration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ProcurementService.API.Service.Registeration
{
    public static class Authentication 
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var appSettings = config.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? new();

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = appSettings.AuthOptions.Issuer,
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = appSettings.AuthOptions.Audience,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    // установка ключа безопасности
                    IssuerSigningKey = appSettings.AuthOptions.SymmetricSecurityKey,
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddHttpContextAccessor();

            services.AddAuthorization();
        }
    }
}
