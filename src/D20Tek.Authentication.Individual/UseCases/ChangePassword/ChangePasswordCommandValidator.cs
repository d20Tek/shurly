//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.ChangePassword;

internal class ChangePasswordCommandValidator : IValidator<ChangePasswordCommand>
{
    public ValidationsResult Validate(ChangePasswordCommand command)
    {
        var result = new ValidationsResult();
        result.AddOnFailure(
        () => command.UserId.NotEmpty(),
            Errors.UserAccount.IdInvalid);

        result.AddOnFailure(
        () => command.CurrentPassword.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.CurrentPassword)));

        result.AddOnFailure(
            () => command.CurrentPassword.HasLength(
                UserAccountConstants.PasswordMinLength,
                UserAccountConstants.PasswordMaxLength),
            Errors.UserAccount.PasswordLength);

        result.AddOnFailure(
        () => command.NewPassword.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.NewPassword)));

        result.AddOnFailure(
            () => command.NewPassword.HasLength(
                UserAccountConstants.PasswordMinLength,
                UserAccountConstants.PasswordMaxLength),
            Errors.UserAccount.PasswordLength);

        return result;
    }
}
