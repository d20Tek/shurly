//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Authentication.Individual.Infrastructure;
using D20Tek.Authentication.Individual.UseCases.ChangePassword;
using D20Tek.Authentication.Individual.UseCases.ChangeRole;
using D20Tek.Authentication.Individual.UseCases.GetById;
using D20Tek.Authentication.Individual.UseCases.Login;
using D20Tek.Authentication.Individual.UseCases.RefreshToken;
using D20Tek.Authentication.Individual.UseCases.Register;
using D20Tek.Authentication.Individual.UseCases.RemoveAccount;
using D20Tek.Authentication.Individual.UseCases.ResetPassword;
using D20Tek.Authentication.Individual.UseCases.UpdateAccount;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Authentication.Individual;

public static partial class DependencyInjection
{
    private const string _defaultDbConnectionKey = "DefaultConnection";

    public static IServiceCollection AddIndividualAuthentication(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDatabaseServices(configuration)
                .AddAuthConfiguration(configuration)
                .AddInfrastructureServices()
                .AddUseCases();

        return services;
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeFacade, DateTimeFacade>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();

        return services;
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ILoginQueryHandler, LoginQueryHandler>();
        services.AddScoped<IRegisterCommandHandler, RegisterCommandHandler>();
        services.AddScoped<IChangePasswordCommandHandler, ChangePasswordCommandHandler>();
        services.AddScoped<IChangeRoleCommandHandler, ChangeRoleCommandHandler>();
        services.AddScoped<IRemoveCommandHandler, RemoveCommandHandler>();
        services.AddScoped<IGetByIdQueryHandler, GetByIdQueryHandler>();
        services.AddScoped<IUpdateCommandHandler, UpdateCommandHandler>();
        services.AddScoped<IRefreshTokenQueryHandler, RefreshTokenQueryHandler>();
        services.AddScoped<IGetResetTokenQueryHandler, GetResetTokenQueryHandler>();
        services.AddScoped<IResetPasswordCommandHandler, ResetPasswordCommandHandler>();

        services.AddScoped<LoginQueryValidator>();
        services.AddScoped<RegisterCommandValidator>();
        services.AddScoped<ChangePasswordCommandValidator>();
        services.AddScoped<ChangeRoleCommandValidator>();
        services.AddScoped<UpdateCommandValidator>();
        services.AddScoped<GetResetTokenQueryValidator>();
        services.AddScoped<ResetPasswordCommandValidator>();

        return services;
    }
}
