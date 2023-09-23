//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.ChangeRole;

internal class ChangeRoleCommandHandler : IChangeRoleCommandHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ChangeRoleCommandValidator _validator;
    private readonly ILogger _logger;

    public ChangeRoleCommandHandler(
        IUserAccountRepository accountRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ChangeRoleCommandValidator validator,
        ILogger<ChangeRoleCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(
        ChangeRoleCommand command,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync(
            _logger,
            async () =>
            {
                // 1. test guard conditions
                var guardResult = await ValidateGuardConditions(command);
                if (guardResult.IsFailure)
                {
                    return guardResult.ErrorsList;
                }

                // 2. convert the user's role
                var account = guardResult.Value;
                return await ConvertRole(account, command.NewRole);
            });
    }

    private async Task<Result<UserAccount>> ValidateGuardConditions(ChangeRoleCommand command)
    {
        // 1. validate command input
        var vResult = _validator.Validate(command);
        if (vResult.IsValid is false)
        {
            return vResult.ToResult<UserAccount>();
        }

        // 2. validate user exists
        if (await _accountRepository.GetByUserNameAsync(command.UserName) is not UserAccount account)
        {
            return Errors.UserAccount.NotFound(command.UserName);
        }

        return account;
    }

    private async Task<Result> ConvertRole(UserAccount account, string newRole)
    {
        // 1. get existing roles
        var existingRoles = (await _accountRepository.GetUserRolesAsync(account)).ToList();

        // 2. add new role
        if (existingRoles.Any(x => x == newRole) is false)
        {
            if (await _accountRepository.AttachUserRoleAsync(account, newRole) is false)
            {
                return Errors.UserAccount.CannotAttachRole;
            }
        }

        // 3. remove previous roles
        existingRoles.Remove(newRole);
        if (existingRoles.Count > 0)
        {
            if (await _accountRepository.RemoveUserRolesAsync(account, existingRoles) is false)
            {
                return Errors.UserAccount.CannotRemoveRoles;
            }
        }

        return Result.Success();
    }
}
