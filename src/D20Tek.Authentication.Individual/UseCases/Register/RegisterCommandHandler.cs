//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.Register;

internal sealed class RegisterCommandHandler : IRegisterCommandHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly RegisterCommandValidator _validator;
    private readonly ILogger _logger;

    public RegisterCommandHandler(
        IUserAccountRepository accountRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        RegisterCommandValidator validator,
        ILogger<RegisterCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResult>> HandleAsync(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AuthenticationResult>(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var guardResult = await ValidateGuardConditions(command);
                if (guardResult.IsFailure)
                {
                    return guardResult.ErrorsList;
                }

                // 2. create user account (generate guid)
                var account = new UserAccount
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = command.UserName,
                    GivenName = command.GivenName,
                    FamilyName = command.FamilyName,
                    Email = command.Email,
                    EmailConfirmed = true,
                    PhoneNumber = command.PhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                // 3. use Identity system to persist individual account in the repository
                var result = await _accountRepository.CreateAsync(account, command.Password);
                if (result.Succeeded is false)
                {
                    return result.ToMinimalResult<AuthenticationResult>();
                }

                // 4. add user role to this account
                if (await _accountRepository.AttachUserRoleAsync(account, UserRoles.User) is false)
                {
                    return Errors.UserAccount.CannotAttachRole;
                }

                return await _jwtTokenGenerator.GenerateTokenResult(_accountRepository, account);
            });
    }

    private async Task<Result> ValidateGuardConditions(RegisterCommand command)
    {
        // 1. validate command input
        var vResult = _validator.Validate(command);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<AuthenticationResult>();
        }

        // 2. check if user already exists
        if (await _accountRepository.GetByUserNameAsync(command.UserName) is not null)
        {
            return Errors.UserAccount.UserNameAlreadyInUse;
        }

        if (await _accountRepository.GetByEmailAsync(command.Email) is not null)
        {
            return Errors.UserAccount.EmailAlreadyInUse;
        }

        return Result.Success();
    }
}
