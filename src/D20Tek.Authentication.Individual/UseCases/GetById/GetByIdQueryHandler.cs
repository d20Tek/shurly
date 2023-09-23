//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Minimal.Domain;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.UseCases.GetById;

internal class GetByIdQueryHandler : IGetByIdQueryHandler
{
    private readonly IUserAccountRepository _accountRepository;
    private readonly ILogger _logger;

    public GetByIdQueryHandler(
        IUserAccountRepository accountRepository,
        ILogger<GetByIdQueryHandler> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }
    public async Task<Result<AccountResult>> HandleAsync(
        GetByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await UseCaseOperation.InvokeAsync<AccountResult>(
            _logger,
            async () =>
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
            });
    }
}
