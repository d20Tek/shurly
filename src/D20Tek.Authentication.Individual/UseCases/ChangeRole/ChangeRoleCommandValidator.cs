//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.ChangeRole;

internal class ChangeRoleCommandValidator : IValidator<ChangeRoleCommand>
{
    public ValidationsResult Validate(ChangeRoleCommand command
        )
    {
        var result = new ValidationsResult();
        result.AddOnFailure(
        () => command.UserName.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.UserName)));

        result.AddOnFailure(
        () => command.NewRole.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.NewRole)));

        result.AddOnFailure(
        () => command.NewRole == UserRoles.Admin || command.NewRole == UserRoles.User,
            Errors.UserAccount.PropertyEmpty(nameof(command.NewRole)));

        return result;
    }
}
