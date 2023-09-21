//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.Register;

internal sealed class RegisterCommandValidator : IValidator<RegisterCommand>
{
    public ValidationsResult Validate(RegisterCommand command)
    {
        var result = new ValidationsResult();

        result.AddOnFailure(
            () => command.UserName.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.UserName)));

        result.AddOnFailure(
            () => command.UserName.InMaxLength(UserAccountConstants.NamesMaxLength),
            Errors.UserAccount.PropertyTooLong(nameof(command.UserName)));

        result.AddOnFailure(
            () => command.GivenName.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.GivenName)));

        result.AddOnFailure(
            () => command.GivenName.InMaxLength(UserAccountConstants.NamesMaxLength),
            Errors.UserAccount.PropertyTooLong(nameof(command.GivenName)));

        result.AddOnFailure(
            () => command.FamilyName.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.FamilyName)));

        result.AddOnFailure(
            () => command.FamilyName.InMaxLength(UserAccountConstants.NamesMaxLength),
            Errors.UserAccount.PropertyTooLong(nameof(command.FamilyName)));

        result.AddOnFailure(
            () => command.Email.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.Email)));

        result.AddOnFailure(
            () => command.Email.InMaxLength(UserAccountConstants.EmailMaxLength),
            Errors.UserAccount.PropertyTooLong(nameof(command.Email)));

        result.AddOnFailure(
            () => command.Email.IsValidEmailAddress(),
            Errors.UserAccount.EmailInvalidFormat);

        if (command.PhoneNumber is not null)
        {
            result.AddOnFailure(
                () => command.PhoneNumber.InMaxLength(UserAccountConstants.PhoneNumberLength),
                Errors.UserAccount.PropertyTooLong(nameof(command.PhoneNumber)));

            result.AddOnFailure(
                () => command.PhoneNumber.IsValidPhoneNumber(),
                Errors.UserAccount.PhoneInvalidFormat);
        }

        result.AddOnFailure(
            () => command.Password.NotEmpty(),
            Errors.UserAccount.PropertyEmpty(nameof(command.Password)));

        result.AddOnFailure(
            () => command.Password.HasLength(
                UserAccountConstants.PasswordMinLength,
                UserAccountConstants.PasswordMaxLength),
            Errors.UserAccount.PasswordLength);

        return result;
    }
}
