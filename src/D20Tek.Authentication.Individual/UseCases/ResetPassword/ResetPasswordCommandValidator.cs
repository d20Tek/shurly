//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.ResetPassword;

internal class ResetPasswordCommandValidator : IValidator<ResetPasswordCommand>
{
    public ValidationsResult Validate(ResetPasswordCommand command)
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

        result.AddOnFailure(
            () => command.ResetToken.NotEmpty(),
                Errors.UserAccount.PropertyEmpty(nameof(command.ResetToken)));

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
