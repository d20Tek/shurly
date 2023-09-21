//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.ResetPassword;

public interface IResetPasswordCommandHandler :
    ICommandHandler<ResetPasswordCommand, Result<AuthenticationResult>>
{
}
