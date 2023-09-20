//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases;
using D20Tek.Minimal.Endpoints;

namespace D20Tek.Authentication.Individual.Api;

internal sealed class AccountResponseMapper :
    IMapper<AccountResult, AccountResponse>
{
    public AccountResponse Map(AccountResult source)
    {
        return new AccountResponse(
            source.UserId.ToString(),
            source.UserName,
            source.GivenName,
            source.FamilyName,
            source.Email,
            source.PhoneNumber);
    }
}
