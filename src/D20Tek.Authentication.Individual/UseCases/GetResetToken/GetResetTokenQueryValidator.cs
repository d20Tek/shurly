//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.ResetPassword;

internal sealed class GetResetTokenQueryValidator : IValidator<GetResetTokenQuery>
{
    public ValidationsResult Validate(GetResetTokenQuery command)
    {
        var result = new ValidationsResult();

        result.AddOnFailure(
            () => command.Email.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.Email)));

        result.AddOnFailure(
            () => command.Email.InMaxLength(UserAccountConstants.EmailMaxLength),
            Errors.UserAccount.PropertyTooLong(nameof(command.Email)));

        result.AddOnFailure(
            () => command.Email.IsValidEmailAddress(),
            Errors.UserAccount.EmailInvalidFormat);

        return result;
    }
}
