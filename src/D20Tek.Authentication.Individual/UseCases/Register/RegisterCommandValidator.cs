//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.Register;

internal sealed class RegisterCommandValidator : IValidator<RegisterCommand>
{
    private const int _namesMaxLength = 64;
    private const int _emailMaxLength = 128;
    private const int _passwordMinLength = 6;
    private const int _passwordMaxLength = 32;

    public ValidationsResult Validate(RegisterCommand command)
    {
        var result = new ValidationsResult();

        result.AddOnFailure(
            () => command.GivenName.NotEmpty(),
            Errors.UserAccount.GivenNameEmpty);

        result.AddOnFailure(
            () => command.GivenName.InMaxLength(_namesMaxLength),
            Errors.UserAccount.GivenNameTooLong);

        result.AddOnFailure(
            () => command.FamilyName.NotEmpty(),
            Errors.UserAccount.FamilyNameEmpty);

        result.AddOnFailure(
            () => command.FamilyName.InMaxLength(_namesMaxLength),
            Errors.UserAccount.FamilyNameTooLong);

        result.AddOnFailure(
            () => command.Email.NotEmpty(),
            Errors.UserAccount.EmailEmpty);

        result.AddOnFailure(
            () => command.Email.InMaxLength(_emailMaxLength),
            Errors.UserAccount.EmailTooLong);

        result.AddOnFailure(
            () => command.Email.IsValidEmailAddress(),
            Errors.UserAccount.EmailInvalidFormat);

        result.AddOnFailure(
            () => command.Password.NotEmpty(),
            Errors.UserAccount.PasswordEmpty);

        result.AddOnFailure(
            () => command.Password.HasLength(_passwordMinLength, _passwordMaxLength),
            Errors.UserAccount.PasswordLength);

        return result;
    }
}
