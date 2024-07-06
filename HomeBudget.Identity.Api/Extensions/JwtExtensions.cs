using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using HomeBudget.Core.Options;
using HomeBudget.Identity.Api.Constants;
using HomeBudget.Identity.Domain;
using HomeBudget.Identity.Domain.Interfaces;

namespace HomeBudget.Identity.Api.Extensions
{
    public static class JwtExtensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new JwtOptions();
            var section = configuration.GetSection(ConfigurationSectionKeys.Jwt);
            section.Bind(options);

            services.Configure<JwtOptions>(section);
            services.AddSingleton<IJwtBuilder, JwtBuilder>();
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret))
                    };
                });
        }
    }
}
