//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.ResetPassword;
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Authentication.Individual.Api;

internal sealed class ResetTokenResponseMapper :
    IMapper<ResetTokenResult, ResetTokenResponse>
{
    public ResetTokenResponse Map(ResetTokenResult source)
    {
        return new ResetTokenResponse(source.ResetCode);
    }
}
