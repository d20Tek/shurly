//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Authentication.Individual;

public static partial class DependencyInjection
{
    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString(_defaultDbConnectionKey) ??
            throw new InvalidOperationException(
                $"Connection string '{_defaultDbConnectionKey}' not found.");

        services.AddDbContext<UserAccountDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("D20Tek.Authentication.Individual")));

        services.AddIdentity<UserAccount, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        })
            .AddEntityFrameworkStores<UserAccountDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddAuthConfiguration(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer([ExcludeFromCodeCoverage] (options) =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    NameClaimType = ClaimTypesExtension.Name,
                    IssuerSigningKey = JwtTokenGenerator.GetSecurityKey(jwtSettings.Secret)
                });

        services.AddAuthorization(config =>
        {
            config.AddPolicy(AuthorizationPolicies.Admin, policyBuilder =>
            {
                policyBuilder.RequireRole(UserRoles.Admin);
                policyBuilder.RequireClaim(ClaimTypesExtension.Scope, jwtSettings.Scopes);
                policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            config.AddPolicy(AuthorizationPolicies.Refresh, policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireClaim(ClaimTypesExtension.Scope, jwtSettings.RefreshScopes);
                policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            config.DefaultPolicy = new AuthorizationPolicy(
                new IAuthorizationRequirement[]
                {
                    new DenyAnonymousAuthorizationRequirement(),
                    new ClaimsAuthorizationRequirement(ClaimTypesExtension.Scope, jwtSettings.Scopes)
                },
                new[] { JwtBearerDefaults.AuthenticationScheme });
        });
        return services;
    }
}
