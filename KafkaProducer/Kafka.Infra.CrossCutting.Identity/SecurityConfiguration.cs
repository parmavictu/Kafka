using System;
using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Kafka.Infra.CrossCutting.Identity.Authorization;
using Kafka.Infra.CrossCutting.Identity.Models;

namespace Kafka.Infra.CrossCutting.Identity.Configurations
{
    public static class SecurityConfiguration
    {
        public static void AddMvcSecurity(this IServiceCollection services, IConfigurationRoot configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var tokenConfigurations = new TokenDescriptor()
            {
                Audience = Environment.GetEnvironmentVariable("TOKEN_SERVER"),
                Issuer = Environment.GetEnvironmentVariable("URL_SERVER"),
                MinutesValid = int.Parse(Environment.GetEnvironmentVariable("MINUTES_VALID"))
            };
            services.AddSingleton(tokenConfigurations);


            services.AddIdentityMongoDbProvider<ApplicationUser>(identityOptions =>
            {
                //identityOptions.SignIn.RequireConfirmedEmail = false;
                //identityOptions.Password.RequiredLength = 5;
                //identityOptions.Password.RequiredUniqueChars = 0;
            }
            , options =>
             {
                 options.ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

             });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;

                var paramsValidation = bearerOptions.TokenValidationParameters;

                paramsValidation.IssuerSigningKey = SigningCredentialsConfiguration.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;

            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("CanReadStudents", policy => policy.RequireClaim("Student", "Read"));
                //options.AddPolicy("CanWriteStudents", policy => policy.RequireClaim("Student", "Write"));

                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }
    }
}