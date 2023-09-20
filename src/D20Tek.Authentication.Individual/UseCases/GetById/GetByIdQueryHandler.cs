//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Result;

namespace D20Tek.Authentication.Individual.UseCases.GetById;

internal class GetByIdQueryHandler : IGetByIdQueryHandler
{
    private readonly IUserAccountRepository _accountRepository;

    public GetByIdQueryHandler(IUserAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    public async Task<Result<AccountResult>> HandleAsync(
        GetByIdQuery query,
        CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetByIdAsync(query.UserId) is not UserAccount account)
        {
            return Errors.UserAccount.NotFound(query.UserId);
        }

        return new AccountResult(
            new Guid(account.Id),
            account.UserName,
            account.GivenName,
            account.FamilyName,
            account.Email,
            account.PhoneNumber);
    }
}
