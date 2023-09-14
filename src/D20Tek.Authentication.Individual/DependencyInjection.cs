﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Authentication.Individual.Infrastructure;
using D20Tek.Authentication.Individual.UseCases.Login;
using D20Tek.Authentication.Individual.UseCases.Register;
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

public static class DependencyInjection
{
    private const string _defaultDbConnectionKey = "DefaultConnection";
    private const string _userPolicyName = "AuthZUser";
    private const string _adminPolicyName = "AuthZAdmin";
    private const string _refreshPolicyName = "AuthZRefresh";

    public static IServiceCollection AddIndividualAuthentication(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDatabaseServices(configuration)
                .AddInfrastructureServices(configuration)
                .AddUseCases();

        return services;
    }

    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString(_defaultDbConnectionKey) ??
            throw new InvalidOperationException(
                $"Connection string '{_defaultDbConnectionKey}' not found.");

        services.AddDbContext<UserAccountDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<UserAccount, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
            .AddEntityFrameworkStores<UserAccountDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeFacade, DateTimeFacade>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();

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
            config.AddPolicy(_adminPolicyName, policyBuilder =>
            {
                policyBuilder.RequireRole(UserRoles.Admin);
                policyBuilder.RequireClaim(ClaimTypesExtension.Scope, jwtSettings.Scopes);
                policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            config.AddPolicy(_refreshPolicyName, policyBuilder =>
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

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ILoginQueryHandler, LoginQueryHandler>();
        services.AddScoped<IRegisterCommandHandler, RegisterCommandHandler>();

        services.AddScoped<LoginQueryValidator>();
        services.AddScoped<RegisterCommandValidator>();

        return services;
    }
}
