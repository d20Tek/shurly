//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.Login;

internal class LoginQueryValidator : IValidator<LoginQuery>
{
    public ValidationsResult Validate(LoginQuery query)
    {
        var result = new ValidationsResult();

        result.AddOnFailure(
            () => query.UserName.NotEmpty(),
            Errors.UserAccount.PropertyEmpty("UserName"));

        result.AddOnFailure(
            () => query.UserName.InMaxLength(UserAccountConstants.NamesMaxLength),
            Errors.UserAccount.PropertyTooLong("UserName"));

        result.AddOnFailure(
            () => query.Password.NotEmpty(),
            Errors.UserAccount.PropertyEmpty("Password"));

        result.AddOnFailure(
            () => query.Password.HasLength(
                UserAccountConstants.PasswordMinLength,
                UserAccountConstants.PasswordMaxLength),
            Errors.UserAccount.PasswordLength);

        return result;
    }
}
